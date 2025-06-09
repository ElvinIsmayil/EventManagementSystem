(function () {
    "use strict";

    const defaultThemeMode = 'light'; // default if no preference is found

    // Function to apply the theme class to the documentElement
    const applyTheme = (mode) => {
        // Remove existing theme classes
        document.documentElement.classList.remove('light', 'dark');
        // Add the new theme class
        document.documentElement.classList.add(mode);
    };

    // --- 1. Initial Theme Load (on page load) ---
    let initialThemeMode;
    if (localStorage.getItem('theme')) {
        initialThemeMode = localStorage.getItem('theme');
    } else if (document.documentElement.hasAttribute('data-theme-mode')) {
        initialThemeMode = document.documentElement.getAttribute('data-theme-mode');
    } else {
        initialThemeMode = defaultThemeMode;
    }

    // Handle 'system' preference
    if (initialThemeMode === 'system') {
        initialThemeMode = window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    }

    applyTheme(initialThemeMode); // Apply the initial theme

    // --- 2. Handle Theme Toggle (on user interaction) ---
    const themeToggle = document.getElementById('kt_user_menu_dark_mode_toggle');

    if (themeToggle) {
        // Set initial state of the checkbox based on the theme applied
        if (initialThemeMode === 'dark') {
            themeToggle.checked = true;
        } else {
            themeToggle.checked = false;
        }

        themeToggle.addEventListener('change', function () {
            let newThemeMode;
            if (this.checked) { // If checkbox is checked, user wants dark mode
                newThemeMode = 'dark';
            } else { // If checkbox is unchecked, user wants light mode
                newThemeMode = 'light';
            }

            // Update localStorage
            localStorage.setItem('theme', newThemeMode);

            // Apply the new theme to the document
            applyTheme(newThemeMode);

            // OPTIONAL: If you want to show a toast message on theme change
            // if (typeof toastr !== 'undefined') {
            //     toastr.info('Theme changed to ' + newThemeMode.charAt(0).toUpperCase() + newThemeMode.slice(1));
            // }

            // IMPORTANT: Remove the Metronic specific redirection logic
            // The data-kt-url attribute and the window.location redirect from widgets.js
            // are for a different kind of theme switching. You'll need to prevent
            // that from overriding this dynamic behavior.
        });
    }

})();

// Ensure this runs when the DOM is ready if it's not placed at the end of the body
// KTUtil.onDOMContentLoaded(function() { /* your script content here if needed */ });
// Or simply include it after your HTML for the toggle.