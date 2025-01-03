


async function fetchRecordEntriesFromApi1() {
    const response = await fetch('https://myskindiary.runasp.net/api/RecordApi/');
    if (!response.ok) {
        console.error("Chyba při načítání dat z API 1");
        return [];
    }
    const data = await response.json();

    return data.map(entry => ({
        date: new Date(entry.created).toISOString().split('T')[0],
        physicalState: entry.physicalState,
        mentalState: entry.mentalState,
        digesting: entry.digesting,
        menstruation: entry.menstruation,
        skinStates: [
            entry.skinState1 || 0,
            entry.skinState2 || 0,
            entry.skinState3 || 0,
            entry.skinState4 || 0,
            entry.skinState5 || 0
        ]
    }));

    //// Seskupení fyzického stavu podle data

    //const groupedData = data.reduce((acc, entry) => {
    //    const date = new Date(entry.created).toISOString().split('T')[0];
    //    if (!acc[date]) acc[date] = [];
    //    acc[date].push(entry.physicalState);
    //    return acc;
    //}, {});

    //// Vytvoření průměrných hodnot fyzického stavu za každý den

    //return Object.entries(groupedData).map(([date, states]) => ({
    //    date,
    //    value: states.reduce((sum, state) => sum + state, 0) / states.length // Průměr hodnot
    //}));
}

// Funkce pro výpočet klouzavého průměru
 function calculateMovingAverage(data, windowSize) {
    const result = [];
    for (let i = 0; i < data.length; i++) {
        if (i < windowSize - 1) {
            result.push(null); // Nedostatečný počet dat
        } else {
            const windowData = data.slice(i - windowSize + 1, i + 1);
            const avg = windowData.reduce((sum, val) => sum + val, 0) / windowSize;
            result.push(avg);
        }
    }
    return result;
}

//async function fetchWeatherData() {
//    const response = await fetch('https://archive-api.open-meteo.com/v1/archive?latitude=49.6833&longitude=18.35&start_date=2024-01-01&end_date=2024-12-02&hourly=temperature_2m');
//    if (!response.ok) {
//        console.error("Chyba při načítání dat z Open-Meteo API");
//        return [];
//    }
//    const data = await response.json();

//    // Seskupení teplot podle data
//    const groupedData = data.hourly.time.reduce((acc, time, index) => {
//        const date = new Date(time).toISOString().split('T')[0]; // ISO datum
//        if (!acc[date]) acc[date] = [];
//        acc[date].push(data.hourly.temperature_2m[index]);
//        return acc;
//    }, {});

//    // Vytvoření průměrných hodnot teplot za každý den
//    return Object.entries(groupedData).map(([date, temperatures]) => ({
//        date,
//        value: temperatures.reduce((sum, temp) => sum + temp, 0) / temperatures.length // Průměr hodnot
//    }));
//}



let chartInstance1;
let chartInstance2;
let chartInstance3;

async function createChart(filteredDates = null) {
    // Načtení dat pouze z API 1
    const api1Data = await fetchRecordEntriesFromApi1();

    // Kombinace všech dat (datumů) pro osu X
    let allDates = [...new Set(api1Data.map(entry => entry.date))];

    if (filteredDates) {
        const { startDate, endDate } = filteredDates;
        allDates = allDates.filter(date => new Date(date) >= new Date(startDate) && new Date(date) <= new Date(endDate));
    }
    allDates.sort((a, b) => new Date(a) - new Date(b)); // Seřazení podle data

    // Naplnění hodnot
    const physicalStates = allDates.map(date => {
        const entry = api1Data.find(item => item.date === date);
        return entry ? entry.physicalState : null;
    });
    const mentalStates = allDates.map(date => {
        const entry = api1Data.find(item => item.date === date);
        return entry ? entry.mentalState : null;
    });
    const digesting = allDates.map(date => {
        const entry = api1Data.find(item => item.date === date);
        return entry ? entry.digesting : null;
    });
    const menstruation = allDates.map(date => {
        const entry = api1Data.find(item => item.date === date);
        return entry ? entry.menstruation : null;
    });

    // Výpočet průměru SkinState za poslední 2 týdny
    const skinStateAverages = allDates.map(date => {
        const entries = api1Data.filter(
            item => item.date >= new Date(new Date(date) - 14 * 24 * 60 * 60 * 1000).toISOString().split('T')[0] &&
                item.date <= date
        );
        const skinStateSums = entries.map(e => e.skinStates.reduce((sum, state) => sum + state, 0) / e.skinStates.length);
        return skinStateSums.length ? (skinStateSums.reduce((sum, avg) => sum + avg, 0) / skinStateSums.length) : null;
    });

    // Získání barev z CSS proměnných
    const primaryColor = getComputedStyle(document.documentElement).getPropertyValue('--bs-primary').trim();
    const secondaryColor = getComputedStyle(document.documentElement).getPropertyValue('--bs-danger').trim();

    // Vytvoření grafu
    const ctx = document.getElementById('recordEntriesChart').getContext('2d');
    if (chartInstance1) chartInstance1.destroy(); // Zničení starého grafu před vytvořením nového
    chartInstance1 = new Chart(ctx, {
        type: 'line', // Typ grafu: čárový
        data: {
            labels: allDates, // Hodnoty na ose X (datumy)
            datasets: [
                { label: 'Physical State', data: physicalStates, borderColor: primaryColor, yAxisID: 'y1', tension: 0.4 },
                { label: 'Mental State', data: mentalStates, borderColor: secondaryColor, yAxisID: 'y1', tension: 0.4 },
                { label: 'Digesting', data: digesting, borderColor: 'green', yAxisID: 'y1', tension: 0.4 },
                { label: 'Menstruation', data: menstruation, borderColor: 'purple', yAxisID: 'y1', tension: 0.4 },
                { label: 'Skin State (Moving Avg)', data: skinStateAverages, tickness: 2, borderColor: 'orange', yAxisID: 'y2', tension: 0.4 }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false, // Dynamická změna rozměrů
            plugins: {
                legend: { position: 'top' }, // Umístění legendy
                tooltip: { enabled: true }, // Aktivace tooltipů
                zoom: {
                    zoom: {
                        wheel: {
                            enabled: true, // Povolení zoomu kolečkem myši
                        },
                        mode: 'x', // Zoom pouze na ose X
                    },
                    pan: {
                        enabled: true, // Povolení posuvu
                        mode: 'x', // Posuv pouze na ose X
                        threshold: 5, // Minimální posuv myší (v pixelech) pro aktivaci
                    }
                }
            },
            scales: {
                x: {
                    title: { display: true, text: 'Date' } // Titulek osy X
                },
                y1: { // Konfigurace první osy Y (stavy)
                    type: 'linear',
                    position: 'left',
                    title: { display: true, text: 'States' },
                    beginAtZero: true,
                    ticks: { stepSize: 1 } // Krokování
                },
                y2: { // Konfigurace druhé osy Y (Skin State Avg)
                    type: 'linear',
                    position: 'right',
                    title: { display: true, text: 'Skin State (Moving Avg)' },
                    beginAtZero: false // Nezačínáme od nuly
                }
            }
        }
    });

    const ctx2 = document.getElementById('recordEntriesChart2').getContext('2d');
    if (chartInstance2) chartInstance2.destroy(); // Zničení starého grafu před vytvořením nového
    chartInstance2 = new Chart(ctx2, {
        type: 'bar', // Hlavní typ grafu pro sloupce
        data: {
            labels: allDates, // Hodnoty na ose X (datumy)
            datasets: [
                {
                    label: 'Physical State',
                    data: physicalStates,
                    type: 'bar', // Sloupcový graf
                    backgroundColor: primaryColor,
                    yAxisID: 'y1'
                },
                {
                    label: 'Mental State',
                    data: mentalStates,
                    type: 'bar', // Sloupcový graf
                    backgroundColor: secondaryColor,
                    yAxisID: 'y1'
                },
                {
                    label: 'Digesting',
                    data: digesting,
                    type: 'bar', // Sloupcový graf
                    backgroundColor: 'green',
                    yAxisID: 'y1'
                },
                {
                    label: 'Menstruation',
                    data: menstruation,
                    type: 'line', // Čárový graf
                    borderColor: 'purple',
                    borderWidth: 2,
                    fill: false,
                    yAxisID: 'y1',
                    tension: 0.4 // Hladká křivka
                },
                {
                    label: 'Skin State (Moving Avg)',
                    data: skinStateAverages,
                    type: 'line', // Čárový graf
                    borderColor: 'orange',
                    borderWidth: 2,
                    fill: false,
                    yAxisID: 'y2',
                    tension: 0.4 // Hladká křivka
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false, // Dynamická změna rozměrů
            plugins: {
                legend: { position: 'top' }, // Umístění legendy
                tooltip: { enabled: true }, // Aktivace tooltipů
                zoom: {
                    zoom: {
                        wheel: {
                            enabled: true, // Povolení zoomu kolečkem myši
                        },
                        mode: 'x', // Zoom pouze na ose X
                    },
                    pan: {
                        enabled: true, // Povolení posuvu
                        mode: 'x', // Posuv pouze na ose X
                        threshold: 5, // Minimální posuv myší (v pixelech) pro aktivaci
                    }
                }
            },
            scales: {
                x: {
                    title: { display: true, text: 'Date' } // Titulek osy X
                },
                y1: { // Konfigurace první osy Y (stavy)
                    type: 'linear',
                    position: 'left',
                    title: { display: true, text: 'States' },
                    beginAtZero: true,
                    ticks: { stepSize: 1 } // Krokování
                },
                y2: { // Konfigurace druhé osy Y (Skin State Avg)
                    type: 'linear',
                    position: 'right',
                    title: { display: true, text: 'Skin State (Moving Avg)' },
                    beginAtZero: false // Nezačínáme od nuly
                }
            }
        }
    });

    const ctx3 = document.getElementById('recordEntriesChart3').getContext('2d');

    // Připrav data pro graf physicalState
    const physicalStateCounts = [0, 0, 0, 0, 0]; // Počítadlo pro hodnoty 1-5
    api1Data.forEach(entry => {
        if (entry.physicalState >= 1 && entry.physicalState <= 5) {
            physicalStateCounts[entry.physicalState - 1]++;
        }
    });

    if (chartInstance3) chartInstance3.destroy(); // Znič starý graf před vytvořením nového
    chartInstance3 = new Chart(ctx3, {
        type: 'doughnut',
        data: {
            labels: ['1 (Very Good)', '2 (Good)', '3 (Neutral)', '4 (Bad)', '5 (Very Bad)'],
            datasets: [{
                label: 'Physical State Distribution',
                data: physicalStateCounts,
                backgroundColor: [
                    'rgba(255, 99, 132, 0.7)',
                    'rgba(54, 162, 235, 0.7)',
                    'rgba(255, 206, 86, 0.7)',
                    'rgba(75, 192, 192, 0.7)',
                    'rgba(153, 102, 255, 0.7)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top'
                },
                tooltip: {
                    enabled: true
                }
            }
        }
    });



    

    document.getElementById('updateChart').addEventListener('click', () => {
        const startDate = document.getElementById('startDate').value;
        const endDate = document.getElementById('endDate').value;

        if (startDate && endDate) {
            createChart({ startDate, endDate });
            // Filtrování tabulky
            const rows = document.querySelectorAll('table tbody tr');
            rows.forEach(row => {
                const rowDate = row.getAttribute('data-date');
                if (rowDate) {
                    const rowDateObj = new Date(rowDate);
                    const startDateObj = new Date(startDate);
                    const endDateObj = new Date(endDate);

                    if (rowDateObj >= startDateObj && rowDateObj <= endDateObj) {
                        row.style.display = ''; // Zobrazit řádek
                    } else {
                        row.style.display = 'none'; // Skrýt řádek
                    }
                }
            });
        } else {
            alert('Please select both start and end dates.');
        }
    });

}

// Vykreslení grafu při načtení stránky
document.addEventListener("DOMContentLoaded", () => createChart());



//async function fetchRecordEntriesFromApi1() {
//    // Načtení dat z API 1
//    const response = await fetch('https://localhost:7212/api/RecordApi/');
//    if (!response.ok) {
//        console.error("Chyba při načítání dat z API 1");
//        return [];
//    }
//    const data = await response.json();
//    console.log("Načtená data z API 1:", data);

//    // Transformace dat: Vytvoření objektů s datem a hodnotou fyzického stavu
//    return data.map(entry => ({
//        date: new Date(entry.created).toString("dd-MM-yyyy"),
//        value: entry.physicalState
//    }));
//}

//async function fetchWeatherData() {
//    // Načtení dat z Open-Meteo API
//    const response = await fetch('https://archive-api.open-meteo.com/v1/archive?latitude=49.6833&longitude=18.35&start_date=2024-01-01&end_date=2024-12-02&hourly=temperature_2m');
//    if (!response.ok) {
//        console.error("Chyba při načítání dat z API Open-Meteo");
//        return [];
//    }
//    const data = await response.json();
//    console.log("Načtená data z Open-Meteo API:", data);

//    // Transformace dat: Vytvoření objektů s datem a teplotou
//    return data.hourly.time.map((time, index) => ({
//        date: new Date(time).toString("dd-MM-yyyy"),
//        value: data.hourly.temperature_2m[index]
//    }));
//}

//async function createChart() {
//    // Načtení dat ze dvou API paralelně
//    const [api1Data, weatherData] = await Promise.all([
//        fetchRecordEntriesFromApi1(),
//        fetchWeatherData()
//    ]);

//    // Kombinace všech dat (datumů) pro osu X
//    const allDates = [...new Set([...api1Data.map(entry => entry.date), ...weatherData.map(entry => entry.date)])];
//    allDates.sort((a, b) => new Date(a) - new Date(b)); // Seřazení podle data

//    // Naplnění hodnot fyzického stavu
//    const physicalStates = allDates.map(date => {
//        const entry = api1Data.find(item => item.date === date);
//        return entry ? entry.value : null; // Pokud nejsou data k dispozici, použijeme null
//    });
//    // Naplnění hodnot teploty
//    const temperatures = allDates.map(date => {
//        const entry = weatherData.find(item => item.date === date);
//        return entry ? entry.value : null; // Pokud nejsou data k dispozici, použijeme null
//    });

//    // Kontrola výsledků
//    console.log("Data pro osu X:", allDates);
//    console.log("Hodnoty fyzického stavu:", physicalStates);
//    console.log("Hodnoty teplot:", temperatures);

//    // Získání barev z CSS proměnných
//    const primaryColor = getComputedStyle(document.documentElement).getPropertyValue('--bs-primary').trim();
//    const secondaryColor = getComputedStyle(document.documentElement).getPropertyValue('--bs-danger').trim();


//    // Vytvoření grafu
//    const ctx = document.getElementById('recordEntriesChart').getContext('2d');
//    new Chart(ctx, {
//        type: 'line', // Typ grafu: čárový
//        data: {
//            labels: allDates, // Hodnoty na ose X (datumy)
//            datasets: [
//                {
//                    label: 'Physical State',
//                    data: physicalStates,
//                    borderColor: primaryColor, // Barva čáry
//                    /*                    backgroundColor: primaryBgColor, // Barva výplně (volitelné)*/
//                    borderWidth: 2,
//                    yAxisID: 'y1', // První osa Y
//                    tension: 0.4 // Mírné zakřivení čáry
//                },
//                {
//                    label: 'Temperature (°C)',
//                    data: temperatures,
//                    borderColor: secondaryColor, // Barva čáry
//                    /*                    backgroundColor: secondaryBgColor,*/
//                    borderWidth: 2,
//                    yAxisID: 'y2', // Druhá osa Y
//                    tension: 0.6 // Mírné zakřivení čáry
//                }
//            ]
//        },
//        options: {
//            responsive: true,
//            maintainAspectRatio: false, // Dynamická změna rozměrů
//            plugins: {
//                legend: { position: 'top' }, // Umístění legendy
//                tooltip: { enabled: true }, // Aktivace tooltipů
//                zoom: {
//                    zoom: {
//                        wheel: {
//                            enabled: true, // Povolení zoomu kolečkem myši
//                        },
//                        mode: 'x', // Zoom pouze na ose X
//                    },
//                    pan: {
//                        enabled: true, // Povolení posuvu
//                        mode: 'x', // Posuv pouze na ose X
//                        threshold: 5, // Minimální posuv myší (v pixelech) pro aktivaci
//                    }
//                }
//            },
//            scales: {
//                x: {
//                    title: { display: true, text: 'Date' } // Titulek osy X
//                },
//                y1: { // Konfigurace první osy Y (fyzický stav)
//                    type: 'linear',
//                    position: 'left',
//                    title: { display: true, text: 'Physical State (1–5)' },
//                    beginAtZero: true,
//                    ticks: { stepSize: 1 } // Krokování
//                },
//                y2: { // Konfigurace druhé osy Y (teplota)
//                    type: 'linear',
//                    position: 'right',
//                    title: { display: true, text: 'Temperature (°C)' },
//                    beginAtZero: false // Nezačínáme od nuly kvůli teplotám
//                }
//            }
//        }
//    });
//}

//// Vykreslení grafu po načtení stránky
//document.addEventListener("DOMContentLoaded", createChart);