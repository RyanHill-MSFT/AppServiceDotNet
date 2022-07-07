// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getGreeting() {
    let hr = new Date().getHours();
    if (hr > 5 && hr < 12) {
        return "Good morning";
    } else if (hr < 18) {
        return "Good afternoon";
    } else {
        return "Good evening";
    }
}