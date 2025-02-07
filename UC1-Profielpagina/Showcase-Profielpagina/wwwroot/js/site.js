// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

window.addEventListener('load', function () {
    setTimeout(function () {
        document.getElementById('loader').style.opacity = '0'; // Loader fades out

        setTimeout(function () {
            document.getElementById('loader').style.display = 'none'; // Loader is hidden
            const content = document.getElementById('content');
            content.style.display = 'block'; // Show content (but still transparent)

            setTimeout(function () {
                content.style.opacity = '1'; // Content fades in
                content.style.transform = 'translateY(0)'; // Content slides up
            }, 50); // Small delay to trigger transition
        }, 1000); // Matches fade-out time
    }, 1000); // Ensures the loader stays visible for at least 1 second
});

