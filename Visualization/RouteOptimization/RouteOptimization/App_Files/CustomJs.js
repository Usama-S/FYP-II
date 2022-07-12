// to dismiss alerts after 3 seconds
setTimeout(function () { $('.fadeout-alert').fadeOut('fast') }, 3000);

// to set date in the input field
function setDate(elem) {
    var x = new Date(elem.value);
    document.getElementById('date').value = x.toDateString();
}
