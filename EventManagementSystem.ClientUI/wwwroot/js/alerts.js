function configureToastr() {
    if (typeof toastr !== 'undefined') {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": true,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
    } else {
        console.warn("Toastr is not loaded. Please ensure toastr.min.js is included.");
    }
}

function showToastr(message, type = 'info', title = '') {
    if (typeof toastr !== 'undefined') {
        switch (type) {
            case 'success':
                toastr.success(message, title);
                break;
            case 'error':
                toastr.error(message, title);
                break;
            case 'warning':
                toastr.warning(message, title);
                break;
            case 'info':
                toastr.info(message, title);
                break;
            default:
                toastr.info(message, title);
                break;
        }
    } else {
        console.error("Toastr is not available to show message:", message);
    }
}

function showSwalAlert(icon, title, text, options = {}) {
    if (typeof Swal !== 'undefined') {
        Swal.fire({
            icon: icon,
            title: title,
            text: text,
            ...options
        });
    } else {
        console.error("SweetAlert2 is not available to show alert:", title);
    }
}

function showSwalConfirm(title, text, confirmCallback, icon = 'warning', confirmButtonText = 'Yes', cancelButtonText = 'Cancel') {
    if (typeof Swal !== 'undefined') {
        Swal.fire({
            title: title,
            text: text,
            icon: icon,
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: confirmButtonText,
            cancelButtonText: cancelButtonText
        }).then((result) => {
            if (result.isConfirmed) {
                if (typeof confirmCallback === 'function') {
                    confirmCallback();
                }
            }
        });
    } else {
        console.error("SweetAlert2 is not available for confirmation:", title);
    }
}

configureToastr();