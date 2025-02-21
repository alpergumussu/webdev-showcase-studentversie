document.addEventListener("DOMContentLoaded", function () {
    let form = document.getElementById("contactForm");

    if (!form) {
        console.error("❌ Error: contactForm not found!");
        return;
    }

    form.addEventListener("submit", async function (event) {
        event.preventDefault();
        console.log("📩 Form submitted!");

        // Get reCAPTCHA response
        let recaptchaResponse = grecaptcha.getResponse();
        if (!recaptchaResponse) {
            alert("⚠️ Please complete the reCAPTCHA before submitting.");
            return;
        }

        let formData = {
            firstName: document.getElementById("fname").value,
            lastName: document.getElementById("lname").value,
            email: document.getElementById("email").value,
            phone: document.getElementById("phone").value,
            subject: document.getElementById("subject").value,
            message: document.getElementById("message").value,
            gRecaptchaResponse: recaptchaResponse // ✅ Include reCAPTCHA response
        };

        try {
            let response = await fetch("/Contact/SubmitContactForm", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(formData)
            });

            console.log(`📡 Server Response Status: ${response.status} ${response.statusText}`);
            let responseText = await response.text();
            console.log("📝 Raw Server Response:", responseText || "(Empty Response)");

            if (!response.ok) {
                console.error("❌ Server responded with an error:", response.status);
                alert(`❌ Error ${response.status}: ${responseText}`);
                return;
            }

            let result = responseText ? JSON.parse(responseText) : {};
            console.log("📨 Parsed JSON Response:", result);
            alert("✅ Success: " + (result.message || "Form submitted successfully!"));
            form.reset();
            grecaptcha.reset(); // ✅ Reset reCAPTCHA after successful submission
        } catch (error) {
            console.error("⚡ Fetch Error:", error);
            alert("❌ An error occurred while submitting the form. Check console for details.");
        }
    });

    let closePopupBtn = document.getElementById("closePopup");
    if (closePopupBtn) {
        closePopupBtn.addEventListener("click", function () {
            popup.style.display = "none";
        });
    }
});
