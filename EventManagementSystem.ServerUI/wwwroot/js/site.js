$(document).ready(function () {
        $('.delete-btn').click(function (e) {
            e.preventDefault();

            const button = $(this);
            const id = button.data('id');
            const name = button.data('name');
            const url = button.data('url');
            const token = $('input[name="__RequestVerificationToken"]').val();

            Swal.fire({
                title: 'Are you sure?',
                text: `You are about to delete "${name}"`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'Cancel'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        url: url,
                        type: 'POST',
                        data: {
                            id: id,
                            __RequestVerificationToken: token
                        },
                        success: function (response) {
                            if (response.success) {
                                Swal.fire('Deleted!', response.message, 'success');
                                button.closest('tr').fadeOut(300, function () {
                                    $(this).remove();
                                });
                            } else {
                                Swal.fire('Error!', response.message, 'error');
                            }
                        },
                        error: function (xhr) {
                            Swal.fire('Error!', 'Something went wrong. Try again.', 'error');
                            console.error('Delete failed:', xhr);
                        }
                    });
                }
            });
        });
});


const defaultThemeMode = 'light'; 
let themeMode;

if (document.documentElement) {
    if (localStorage.getItem('theme')) {
        themeMode = localStorage.getItem('theme');
    } else if (document.documentElement.hasAttribute('data-theme-mode')) {
        themeMode = document.documentElement.getAttribute('data-theme-mode');
    } else {
        themeMode = defaultThemeMode;
    }

    if (themeMode === 'system') {
        themeMode = window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    }

    document.documentElement.classList.add(themeMode);
}