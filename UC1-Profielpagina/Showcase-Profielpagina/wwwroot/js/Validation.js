document.addEventListener("DOMContentLoaded", function () {
    // Ensure popup is hidden on page load
    document.getElementById("popup").style.display = "none";

    let form = document.getElementById("contactForm");

    if (!form) {
        console.error("Error: contactForm not found!");
        return;
    }

    form.addEventListener("submit", function (event) {
        event.preventDefault();
        console.log("Form submitted!"); // Debugging

        // Show the popup only on valid submission
        document.getElementById("popup").style.display = "flex";

        // Reset the form
        form.reset();
    });

    document.getElementById("closePopup").addEventListener("click", function () {
        document.getElementById("popup").style.display = "none";
    });
});
