// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function SetCheckboxes(checkbox) {
    var checkboxes = $("input[name='LinkUrls']");
    var checkboxValue = checkbox.checked;

    for (var i = 0; i < checkboxes.length; i++) {
        checkboxes[i].checked = checkboxValue;
    }
}

function SelectLink(url, shortUrl, createdAt, ckicksNumber) {
    var date = new Date(createdAt);

    $("input[name='Url']")[0].value = url;
    $("input[name='ShortUrl']")[0].value = shortUrl;
    $("input[name='CreatedAt']")[0].value = convertToDateTimeLocalString(date);
    $("input[name='ClicksNumber']")[0].value = ckicksNumber;
}

const convertToDateTimeLocalString = (date) => {
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, "0");
    const day = date.getDate().toString().padStart(2, "0");
    const hours = date.getHours().toString().padStart(2, "0");
    const minutes = date.getMinutes().toString().padStart(2, "0");

    return `${year}-${month}-${day}T${hours}:${minutes}`;
}
