document.addEventListener('DOMContentLoaded', function () {

    let cropper;
    // This assumes @Model.ProfilePictureUpload.CurrentImageUrl is rendered by Razor into the JS
    let currentImageUrl = '@Model.ProfilePictureUpload.CurrentImageUrl';

    // File upload handling
    $('#imageUpload').change(function () {
        const file = this.files[0];
        if (!file) return;

        const fileName = file.name;
        $('#fileName').text(fileName);

        // Validate file type
        if (!file.type.match('image.*')) {
            showAlert('Please select an image file (JPG, PNG, GIF)', 'danger');
            return;
        }

        // Validate file size (max 5MB)
        if (file.size > 5 * 1024 * 1024) {
            showAlert('File size must be less than 5MB', 'danger');
            return;
        }

        const reader = new FileReader();
        reader.onload = function (e) {
            // Destroy previous cropper instance if exists
            if (cropper) {
                cropper.destroy();
            }

            // Show the image preview
            $('#imagePreview').attr('src', e.target.result).removeClass('d-none');
            $('#imageToCrop').attr('src', e.target.result); // Set image for Cropper

            // Show the cropper container and zoom controls
            $('#cropperContainer').removeClass('d-none');
            $('#zoomControls').removeClass('d-none');

            // Initialize cropper
            const image = document.getElementById('imageToCrop');
            cropper = new Cropper(image, {
                aspectRatio: 1,      // Forces a square crop area
                viewMode: 1,         // Restricts the crop box to the canvas
                autoCropArea: 0.8,   // 80% of the image will be cropped by default
                responsive: true,
                guides: false        // Hides the dashed guidelines
            });

            // Enable save button
            $('#savePictureBtn').prop('disabled', false);
        };
        reader.readAsDataURL(file);
    });

    // --- Cropper Zoom Controls ---
    $('#zoomIn').click(function () {
        if (cropper) cropper.zoom(0.1);
    });

    $('#zoomOut').click(function () {
        if (cropper) cropper.zoom(-0.1);
    });

    $('#zoomReset').click(function () {
        if (cropper) cropper.reset();
    });

    // --- Save Profile Picture Handler ---
    $('#savePictureBtn').click(function () {
        if (!cropper) {
            showAlert('Please select an image first', 'danger');
            return;
        }

        // Get the cropped canvas
        const canvas = cropper.getCroppedCanvas({
            width: 500,
            height: 500,
            minWidth: 256,
            minHeight: 256,
            maxWidth: 2000,
            maxHeight: 2000,
            fillColor: '#fff',
            imageSmoothingEnabled: true,
            imageSmoothingQuality: 'high'
        });

        if (!canvas) {
            showAlert('Error cropping image. Please try again.', 'danger');
            return;
        }

        // Convert canvas to blob for AJAX upload
        canvas.toBlob(function (blob) {
            // Create a new FormData object from the existing form
            const formData = new FormData($('#profilePictureForm')[0]);

            // Create a File object from the blob, using the original filename
            const file = new File([blob], $('#imageUpload').val().split('\\').pop(), {
                type: 'image/jpeg', // Force JPEG for consistency, adjust if other formats needed
                lastModified: Date.now()
            });

            // Replace the file in FormData with the cropped version
            formData.set('NewImageFile', file);

            // Submit the form with AJAX
            $.ajax({
                url: $('#profilePictureForm').attr('action'),
                type: 'POST',
                data: formData,
                processData: false, // Prevents jQuery from processing the data
                contentType: false, // Prevents jQuery from setting contentType
                success: function (response) {
                    if (response.success) {
                        // Update the profile picture on the page, bust cache with timestamp
                        $('.profile-picture').attr('src', response.newImageUrl + '?' + new Date().getTime());
                        $('#imagePreview').attr('src', response.newImageUrl);
                        currentImageUrl = response.newImageUrl; // Update the current image URL for modal reset

                        showAlert('Profile picture updated successfully!', 'success');
                        $('#profilePictureModal').modal('hide'); // Close the modal
                    } else {
                        showAlert(response.message || 'Error updating profile picture', 'danger');
                    }
                },
                error: function () {
                    showAlert('An error occurred while updating your profile picture', 'danger');
                }
            });
        }, 'image/jpeg', 0.9); // Quality 0.9 for JPEG
    });

    // --- Delete Picture Button Handler ---
    $('#deletePictureBtn').click(function () {
        $('#deleteConfirmationModal').modal('show');
    });

    // After delete, update the UI
    $('#deletePictureForm').submit(function (e) {
        e.preventDefault(); // Prevent default form submission

        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: $(this).serialize(), // Serialize form data (CSRF token etc.)
            success: function (response) {
                if (response.success) {
                    const defaultImage = '/img/default-profile.png'; // Path to your default image
                    $('.profile-picture').attr('src', defaultImage);
                    $('#imagePreview').attr('src', defaultImage);
                    currentImageUrl = ''; // Clear current image URL

                    showAlert('Profile picture removed successfully!', 'success');

                    // Close both modals
                    $('#deleteConfirmationModal').modal('hide');
                    $('#profilePictureModal').modal('hide');

                    // Hide delete button if no picture exists
                    $('#deletePictureBtn').hide();
                } else {
                    showAlert(response.message || 'Error removing profile picture', 'danger');
                }
            },
            error: function () {
                showAlert('An error occurred while removing your profile picture', 'danger');
            }
        });
    });

    // --- Reset modal when closed ---
    $('#profilePictureModal').on('hidden.bs.modal', function () {
        // Reset file input
        $('#imageUpload').val('');
        $('#fileName').text('No file chosen');

        // Reset preview to current image or default
        $('#imagePreview').attr('src', currentImageUrl || '/img/default-profile.png');
        $('#imageToCrop').attr('src', ''); // Clear the cropper image source

        // Hide cropper and controls
        $('#cropperContainer').addClass('d-none');
        $('#zoomControls').addClass('d-none');

        // Destroy cropper if exists
        if (cropper) {
            cropper.destroy();
            cropper = null; // Clear the cropper instance
        }

        // Disable save button
        $('#savePictureBtn').prop('disabled', true);
    });

    // --- Toggle password visibility ---
    $('.toggle-password').click(function () {
        const target = $(this).data('target'); // Get the ID of the input to toggle
        const input = $('#' + target);
        const icon = $(this).find('i');

        if (input.attr('type') === 'password') {
            input.attr('type', 'text');
            icon.removeClass('fa-eye').addClass('fa-eye-slash');
        } else {
            input.attr('type', 'password');
            icon.removeClass('fa-eye-slash').addClass('fa-eye');
        }
    });

    // --- Helper function to show alerts ---
    function showAlert(message, type) {
        const alertHtml = `
            <div class="alert alert-${type} alert-dismissible fade show" role="alert">
                ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>
        `;

        // Remove any existing alerts to prevent stacking
        $('.alert-dismissible').alert('close');

        // Prepend new alert to a suitable location (e.g., after a header or in a dedicated alert area)
        // Adjust '.profile-header' if you have a specific alert container
        $('.profile-header').after(alertHtml);
    }
});