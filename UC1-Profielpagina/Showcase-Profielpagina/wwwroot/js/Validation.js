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
        console.log("Form submitted!"); 

        var captchaResponse = grecaptcha.getResponse();

        if (captchaResponse.length == 0) {
           
            return; 
        }

        document.getElementById("popup").style.display = "flex";

        form.reset();

        grecaptcha.reset()
    });

    document.getElementById("closePopup").addEventListener("click", function () {
        document.getElementById("popup").style.display = "none";
    });
});
