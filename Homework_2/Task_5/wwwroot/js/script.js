window.addEventListener('load', () => {
    fetch("https://countryinfoapi.com/api/countries", {}).then(response => response.json()).then((data) => {
        const countrySelect = document.getElementById('countrySelect');
        const countryInfo = document.getElementById('countryInfo');
        for (var i = 0; i < 5; ++i) {
            var country = data[Math.floor(Math.random() * data.length)]
            const option = document.createElement('option');
            option.value = country.name;
            option.textContent = country.name;
            countrySelect.appendChild(option);
        }

        countrySelect.addEventListener('change', () => {
            const selectedCountry = countrySelect.value;
            const selectedCountryData = data.find(country => country.name === selectedCountry);
            countryInfo.innerHTML = `
                    <h2>${selectedCountryData.name}</h2>
                    <p>Capital: ${selectedCountryData.capital}</p>
                    <p>Population: ${selectedCountryData.population}</p>
                    <p>Region: ${selectedCountryData.region}</p>
                    <p>Subregion: ${selectedCountryData.subregion}</p>
                    <p>Area: ${selectedCountryData.area} km²</p>
                  `;
        });
    });
});