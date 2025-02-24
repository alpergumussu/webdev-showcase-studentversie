window.addEventListener('load', function () {
    setTimeout(function () {
        document.getElementById('loader').style.opacity = '0'; 

        setTimeout(function () {
            document.getElementById('loader').style.display = 'none'; 
            const content = document.getElementById('content');
            content.style.display = 'block';

            setTimeout(function () {
                content.style.opacity = '1';
                content.style.transform = 'translateY(0)';

                const gdpr = new GDPR();

            }, 50);
        }, 500);
    }, 500);
});
