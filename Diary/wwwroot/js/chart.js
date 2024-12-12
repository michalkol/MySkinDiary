
//https://myskindiary.runasp.net/api/RecordApi/

async function fetchRecordEntriesFromApi1() {
    const response = await fetch('https://myskindiary.runasp.net/api/RecordApi/');
    if (!response.ok) {
        console.error("Chyba při načítání dat z API 1");
        return [];
    }
    const data = await response.json();

    // Seskupení fyzického stavu podle data
    const groupedData = data.reduce((acc, entry) => {
        const date = new Date(entry.created).toISOString().split('T')[0]; // ISO datum
        if (!acc[date]) acc[date] = [];
        acc[date].push(entry.physicalState);
        return acc;
    }, {});

    // Vytvoření průměrných hodnot fyzického stavu za každý den
    return Object.entries(groupedData).map(([date, states]) => ({
        date,
        value: states.reduce((sum, state) => sum + state, 0) / states.length // Průměr hodnot
    }));
}

async function fetchWeatherData() {
    const response = await fetch('https://archive-api.open-meteo.com/v1/archive?latitude=49.6833&longitude=18.35&start_date=2024-01-01&end_date=2024-12-02&hourly=temperature_2m');
    if (!response.ok) {
        console.error("Chyba při načítání dat z Open-Meteo API");
        return [];
    }
    const data = await response.json();

    // Seskupení teplot podle data
    const groupedData = data.hourly.time.reduce((acc, time, index) => {
        const date = new Date(time).toISOString().split('T')[0]; // ISO datum
        if (!acc[date]) acc[date] = [];
        acc[date].push(data.hourly.temperature_2m[index]);
        return acc;
    }, {});

    // Vytvoření průměrných hodnot teplot za každý den
    return Object.entries(groupedData).map(([date, temperatures]) => ({
        date,
        value: temperatures.reduce((sum, temp) => sum + temp, 0) / temperatures.length // Průměr hodnot
    }));
}

async function createChart() {
    // Načtení dat ze dvou API paralelně
    const [api1Data, weatherData] = await Promise.all([
        fetchRecordEntriesFromApi1(),
        fetchWeatherData()
    ]);

    // Kombinace všech dat (datumů) pro osu X
    const allDates = [...new Set([...api1Data.map(entry => entry.date), ...weatherData.map(entry => entry.date)])];
    allDates.sort((a, b) => new Date(a) - new Date(b)); // Seřazení podle data

    // Naplnění hodnot fyzického stavu
    const physicalStates = allDates.map(date => {
        const entry = api1Data.find(item => item.date === date);
        return entry ? entry.value : null; // Pokud nejsou data k dispozici, použijeme null
    });
    // Naplnění hodnot teploty
    const temperatures = allDates.map(date => {
        const entry = weatherData.find(item => item.date === date);
        return entry ? entry.value : null; // Pokud nejsou data k dispozici, použijeme null
    });

    // Kontrola výsledků
    console.log("Data pro osu X:", allDates);
    console.log("Hodnoty fyzického stavu:", physicalStates);
    console.log("Hodnoty teplot:", temperatures);

    // Získání barev z CSS proměnných
    const primaryColor = getComputedStyle(document.documentElement).getPropertyValue('--bs-primary').trim();
    const secondaryColor = getComputedStyle(document.documentElement).getPropertyValue('--bs-danger').trim();


    // Vytvoření grafu
    const ctx = document.getElementById('recordEntriesChart').getContext('2d');
    new Chart(ctx, {
        type: 'line', // Typ grafu: čárový
        data: {
            labels: allDates, // Hodnoty na ose X (datumy)
            datasets: [
                {
                    label: 'Physical State',
                    data: physicalStates,
                    borderColor: primaryColor, // Barva čáry
                    /*                    backgroundColor: primaryBgColor, // Barva výplně (volitelné)*/
                    borderWidth: 2,
                    yAxisID: 'y1', // První osa Y
                    tension: 0.4 // Mírné zakřivení čáry
                },
                {
                    label: 'Temperature (°C)',
                    data: temperatures,
                    borderColor: secondaryColor, // Barva čáry
                    /*                    backgroundColor: secondaryBgColor,*/
                    borderWidth: 2,
                    yAxisID: 'y2', // Druhá osa Y
                    tension: 0.6 // Mírné zakřivení čáry
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
                y1: { // Konfigurace první osy Y (fyzický stav)
                    type: 'linear',
                    position: 'left',
                    title: { display: true, text: 'Physical State (1–5)' },
                    beginAtZero: true,
                    ticks: { stepSize: 1 } // Krokování
                },
                y2: { // Konfigurace druhé osy Y (teplota)
                    type: 'linear',
                    position: 'right',
                    title: { display: true, text: 'Temperature (°C)' },
                    beginAtZero: false // Nezačínáme od nuly kvůli teplotám
                }
            }
        }
    });
}

// Vykreslení grafu po načtení stránky
document.addEventListener("DOMContentLoaded", createChart);


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