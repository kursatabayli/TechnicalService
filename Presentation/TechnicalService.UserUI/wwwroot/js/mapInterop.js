let map;
let currentInfoWindow = null;
let markers = [];
function initMap() {
    map = new google.maps.Map(document.getElementById("mapContainer"), {
        center: { lat: 39.3737915, lng: 34.9776577 },
        zoom: 6,
        zoomControl: true,
        mapTypeControl: true,
        scaleControl: true,
        rotateControl: true,
        streetViewControl: false,
        fullscreenControl: false,
        mapTypeControl: false,
        mapTypeId: google.maps.MapTypeId.ROADMAP,

    });
}
function clearMarkers() {
    for (let i = 0; i < markers.length; i++) {
        markers[i].setMap(null);
    }
    markers = [];
}
/**
 * Adresi panoya kopyalayan ve kullanıcıya görsel geri bildirim sağlayan fonksiyon.
 * @param {HTMLButtonElement} buttonElement - Tıklanan butonun kendisi.
 * @param {string} textToCopy - Kopyalanacak metin.
 */
function copyAddressToClipboard(buttonElement, textToCopy) {
    navigator.clipboard.writeText(textToCopy).then(() => {
        const icon = buttonElement.querySelector('.material-icons');
        if (!icon) return;

        buttonElement.disabled = true;

        icon.style.opacity = '0';
        icon.style.transform = 'scale(0.7)';

        setTimeout(() => {
            icon.textContent = 'check';
            icon.style.color = 'var(--mud-palette-success)';
            icon.style.opacity = '1';
            icon.style.transform = 'scale(1)';
        }, 200);

        setTimeout(() => {
            icon.style.opacity = '0';
            icon.style.transform = 'scale(0.7)';

            setTimeout(() => {
                icon.textContent = 'content_copy';
                icon.style.color = '';
                icon.style.opacity = '1';
                icon.style.transform = 'scale(1)';

                buttonElement.disabled = false;
            }, 200);

        }, 2200);

    }).catch(err => {
        console.error('Adres kopyalanamadı: ', err);
        alert('Hata: Adres kopyalanamadı!');
    });
}

function SetMarkers(serviceJson) {

    const service = JSON.parse(serviceJson);

    const { ServiceName, Lat, Lng, Address, District, City, PostalCode } = service;

    const marker = new google.maps.Marker({
        position: { lat: Lat, lng: Lng },
        map: map,
        title: ServiceName,
        icon: {
            url: "http://maps.google.com/mapfiles/ms/icons/red-dot.png",
            scaledSize: new google.maps.Size(32, 32),
            anchor: new google.maps.Point(16, 32)
        }
    });

    markers.push(marker);


    marker.addListener("click", () => {

        if (currentInfoWindow)
            currentInfoWindow.close();

        const addressLines = [Address, `${District} / ${City}`, PostalCode && `Posta Kodu: ${PostalCode}`].filter(Boolean);
        const copyAddressText = addressLines.join(', ');
        const directionsUrl = `https://www.google.com/maps/dir/?api=1&destination=${encodeURIComponent(copyAddressText)}`;
        const content = `<div class="mud-paper mud-elevation-10"
                        style="font-family:var(--mud-typography-default-family);width:300px;
                               background:var(--mud-palette-surface);border-radius:8px;">

                        <div class="d-flex align-center pa-4" style="border-bottom:1px solid var(--mud-palette-lines-default);">
                            <span class="material-icons mr-3" style="color:var(--mud-palette-primary);font-size:28px;">place</span>
                            <div class="mud-typography mud-typography-h6" style="color:var(--mud-palette-text-primary);">
                                ${ServiceName}
                            </div>
                        </div>

                        <div class="pa-4">
                            <div class="d-flex mb-2">
                                <span class="material-icons mr-3" style="color:var(--mud-palette-text-secondary);">location_city</span>
                                <div class="mud-typography mud-typography-body1" style="color:var(--mud-palette-text-secondary);">
                                    ${addressLines.map(line => `<div>${line}</div>`).join('')}
                                </div>
                            </div>
                        </div>

                        <div class="d-flex pa-4" style="border-top:1px solid var(--mud-palette-lines-default);gap:12px;">
                                <a href="${directionsUrl}" target="_blank"
                                   class="mud-button mud-button-text mud-primary-text mud-button-size-medium mud-button-variant-text"
                                   style="text-transform:none;text-decoration:none;">
                                <span class="material-icons mr-2">directions</span>
                                Yol Tarifi Al
                            </a>

                            <div class="flex-grow-1"></div>
                            <button onclick="copyAddressToClipboard(this, '${copyAddressText}')" class="mud-button-root mud-icon-button mud-button-text mud-ripple" title="Adresi Kopyala">
                                <span class="material-icons mud-icon-button-label" style="transition: all 0.2s ease-in-out;">content_copy</span>
                            </button>
                        </div>
                    </div>`;


        const infowindow = new google.maps.InfoWindow({
            content: content
        });


        infowindow.open(map, marker);

        currentInfoWindow = infowindow;
        infowindow.addListener("domready", () => {
            const iwOuter = document.querySelector(".gm-style-iw");

            if (iwOuter) {
                iwOuter.style.backgroundColor = "var(--mud-palette-surface)";
                iwOuter.style.borderRadius = "8px";
                iwOuter.style.border = "1px solid var(--mud-palette-lines-default)";
                iwOuter.style.boxShadow = "var(--mud-elevation-10)";

                const iwBackground = iwOuter.parentElement?.parentElement;
                if (iwBackground) {
                    iwBackground.style.boxShadow = "var(--mud-elevation-10)";
                }
            }


            const closeButton = document.querySelector(".gm-ui-hover-effect");

            if (closeButton) {
                const span = closeButton.querySelector("span");

                if (span)
                    span.remove();

                const newIcon = document.createElement("span");
                newIcon.classList.add("material-icons");
                newIcon.textContent = "close";
                newIcon.style.backgroundColor = "var(--mud-palette-surface)";
                newIcon.style.color = "var(--mud-palette-primary)";
                newIcon.style.fontSize = "28px";
                newIcon.style.fontWeight = "bold";
                closeButton.appendChild(newIcon);

            }

        });

    });

}
