document.addEventListener("DOMContentLoaded", function () {
    // Ensure popup is hidden on page load
    let popup = document.getElementById("popup");
    if (popup) popup.style.display = "none";

    let form = document.getElementById("contactForm");

    if (!form) {
        console.error("Error: contactForm not found!");
        return;
    }

    form.addEventListener("submit", function (event) {
        event.preventDefault(); // Prevent default submission until validation passes
        console.log("Form submitted!");

        var captchaResponse = grecaptcha.getResponse();

        if (captchaResponse.length === 0) {
            alert("Please verify that you are not a robot.");
            return; // Stop submission if reCAPTCHA is not completed
        }

        // Set hidden input field with reCAPTCHA response
        document.getElementById("gRecaptchaResponse").value = captchaResponse;

        // Show popup after successful submission
        if (popup) popup.style.display = "flex";

        // Reset form fields and reCAPTCHA
        form.reset();
        grecaptcha.reset();
    });

    let closePopupBtn = document.getElementById("closePopup");
    if (closePopupBtn) {
        closePopupBtn.addEventListener("click", function () {
            popup.style.display = "none";
        });
    }
});
