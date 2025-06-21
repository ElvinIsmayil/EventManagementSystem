document.addEventListener("DOMContentLoaded", function () {
    // --- Individual Delete Button Logic (Using Event Delegation) ---
    // This block handles clicks on any element with class 'delete-btn' across the document.
    // This is robust for elements that are present on page load or added dynamically later.
    document.body.addEventListener('click', function (e) {
        const button = e.target.closest('.delete-btn');
        if (button) {
            e.preventDefault(); // Prevent default link behavior (e.g., navigating to #)

            const id = button.dataset.id;
            const name = button.dataset.name;
            const url = button.dataset.url;
            // Safely get anti-forgery token, check if jQuery is available as it's typically used for this
            const token = typeof $ !== 'undefined' ? $('input[name="__RequestVerificationToken"]').val() : '';

            // Use SweetAlert2 for confirmation if available, otherwise fallback to native confirm
            if (typeof Swal === 'undefined') {
                console.error("SweetAlert2 is not loaded. Please ensure its script is included.");
                if (confirm(`Are you sure you want to delete "${name}"?`)) {
                    // Fallback to fetch API if SweetAlert2 is not available
                    performDeleteFetch(url, id, token, button.closest('tr')); // Pass the row for removal
                }
                return; // Exit function if Swal is not defined
            }

            Swal.fire({
                title: 'Are you sure?',
                text: `You are about to delete "${name}"`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'Cancel'
            }).then((result) => {
                if (result.isConfirmed) {
                    performDeleteFetch(url, id, token, button.closest('tr')); // Pass the row for removal
                }
            });
        }
    });

    // --- Helper function for making the delete fetch call ---
    // Consolidated logic for both individual and batch deletion
    function performDeleteFetch(url, idOrIds, token, elementToRemove = null) {
        // Determine if it's a single ID or an array of IDs based on the input type
        const isArray = Array.isArray(idOrIds);
        const dataToSend = isArray ? idOrIds : parseInt(idOrIds); // Ensure single ID is an integer

        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token // Include the anti-forgery token in the header
            },
            body: JSON.stringify(dataToSend) // Send data as JSON
        })
            .then(response => {
                // Check if the HTTP response status indicates an error
                if (!response.ok) {
                    // Attempt to parse JSON error message from the response body if available
                    return response.json().then(err => Promise.reject(err.message || response.statusText));
                }
                return response.json(); // Parse the successful response as JSON
            })
            .then(data => {
                // Handle success response from the server
                if (data.success) {
                    // Use Toastr for a non-blocking success message if available
                    if (typeof toastr !== 'undefined') {
                        toastr.success(data.message);
                    } else if (typeof Swal !== 'undefined') {
                        // Fallback to SweetAlert2 for success if Toastr is not available
                        Swal.fire('Deleted!', data.message, 'success');
                    } else {
                        // Final fallback to native alert
                        alert(data.message);
                    }

                    // If an element was provided to remove (e.g., a table row for individual delete)
                    if (elementToRemove && !isArray && typeof $ !== 'undefined' && $.fn.fadeOut) {
                        // Use jQuery's fadeOut for a smoother removal animation
                        $(elementToRemove).fadeOut(300, function () {
                            $(this).remove(); // Remove the element from the DOM after fading out
                            // If on an index page, update the selected count after removing a row
                            if (document.getElementById('selected-count')) {
                                updateSelectedCount();
                            }
                        });
                    } else {
                        // For batch delete or if no specific element to remove, just reload the page
                        location.reload();
                    }
                } else {
                    // Handle error response from the server
                    if (typeof toastr !== 'undefined') {
                        toastr.error(data.message);
                    } else if (typeof Swal !== 'undefined') {
                        Swal.fire('Error!', data.message, 'error');
                    } else {
                        alert('Error: ' + data.message);
                    }
                }
            })
            .catch(error => {
                // Catch and log any errors during the fetch operation
                console.error('Delete failed:', error);
                if (typeof toastr !== 'undefined') {
                    toastr.error('An error occurred during deletion: ' + error.message);
                } else if (typeof Swal !== 'undefined') {
                    Swal.fire('Error!', 'An error occurred during deletion. See console for details.', 'error');
                } else {
                    alert('An error occurred during deletion.');
                }
            });
    }

    // --- Index Page Specific Logic (Select All / Batch Delete) ---
    // Conditionally get elements that only exist on the Index page
    const selectAllCheckbox = document.getElementById('select-all-checkbox');
    const rowCheckboxes = document.querySelectorAll('.row-checkbox');
    const selectedToolbar = document.getElementById('selected-toolbar');
    const selectedCountSpan = document.getElementById('selected-count');
    const deleteSelectedBtn = document.getElementById('delete-selected-btn');

    // Only execute this block if the necessary elements for the Index page exist
    if (selectedCountSpan && selectedToolbar) {
        function updateSelectedCount() {
            const checkedCount = document.querySelectorAll('.row-checkbox:checked').length;
            selectedCountSpan.textContent = checkedCount;

            if (checkedCount > 0) {
                selectedToolbar.classList.remove('d-none');
            } else {
                selectedToolbar.classList.add('d-none');
            }

            // Update the state of the "select all" checkbox
            if (selectAllCheckbox && rowCheckboxes.length > 0) {
                selectAllCheckbox.checked = (checkedCount === rowCheckboxes.length);
            }
        }

        // Add event listeners to individual row checkboxes
        rowCheckboxes.forEach(checkbox => {
            checkbox.addEventListener('change', updateSelectedCount);
        });

        // Add event listener to the "select all" checkbox
        if (selectAllCheckbox) { // Ensure selectAllCheckbox exists before adding listener
            selectAllCheckbox.addEventListener('change', function () {
                rowCheckboxes.forEach(checkbox => {
                    checkbox.checked = this.checked;
                });
                updateSelectedCount(); // Update count and toolbar after (de)selecting all
            });
        }

        // Event listener for the "Delete Selected" button
        if (deleteSelectedBtn) { // Ensure deleteSelectedBtn exists before adding listener
            deleteSelectedBtn.addEventListener('click', function () {
                const selectedIds = [];
                document.querySelectorAll('.row-checkbox:checked').forEach(checkbox => {
                    selectedIds.push(parseInt(checkbox.value));
                });

                if (selectedIds.length === 0) {
                    if (typeof toastr !== 'undefined') {
                        toastr.warning("No items selected for deletion.");
                    } else {
                        alert("No items selected for deletion.");
                    }
                    return;
                }

                // Use SweetAlert2 for confirmation if available
                if (typeof Swal === 'undefined') {
                    console.error("SweetAlert2 is not loaded. Please ensure its script is included.");
                    if (confirm(`Are you sure you want to delete ${selectedIds.length} selected item(s)?`)) {
                        performDeleteFetch(this.dataset.url, selectedIds, document.querySelector('input[name="__RequestVerificationToken"]').value, null);
                    }
                    return;
                }

                Swal.fire({
                    title: 'Are you sure?',
                    text: `You are about to delete ${selectedIds.length} selected item(s). This action cannot be undone!`,
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#d33',
                    cancelButtonColor: '#3085d6',
                    confirmButtonText: 'Yes, delete selected!'
                }).then((result) => {
                    if (result.isConfirmed) {
                        performDeleteFetch(this.dataset.url, selectedIds, document.querySelector('input[name="__RequestVerificationToken"]').value, null);
                    }
                });
            });
        }

        // Initial update when the page loads (for index pages only)
        updateSelectedCount();
    }


    // --- Photo Management Logic (for Create/Update forms) ---
    // Conditionally get elements that only exist on Create/Update pages for photo management
    const photosContainer = document.getElementById('photos-container');
    const addPhotoBtn = document.getElementById('add-photo-btn');

    // Only execute this block if the necessary elements for photo management exist
    if (photosContainer && addPhotoBtn) {
        // Retrieve initial photo count from a data attribute or default to 0
        // (You would add data-initial-photo-count="@(Model?.EventPhotos?.Count ?? 0)" to #photos-container)
        let photoIndex = parseInt(photosContainer.dataset.initialPhotoCount || '0');

        function updatePhotoNumbers() {
            const photoItems = photosContainer.querySelectorAll('.photo-item');
            photoItems.forEach((item, index) => {
                // Update visible photo number (e.g., "Photo #1")
                const photoNumberSpan = item.querySelector('.photo-number');
                if (photoNumberSpan) {
                    photoNumberSpan.textContent = index + 1;
                }

                // Update input names and data-valmsg-for attributes for ASP.NET Core model binding
                item.querySelectorAll('[name^="EventPhotos["]').forEach(input => {
                    const originalName = input.name;
                    // Regex to replace the index part of "EventPhotos[X]"
                    const newName = originalName.replace(/EventPhotos\[\d+\]/, `EventPhotos[${index}]`);
                    input.name = newName;

                    // Update associated validation message spans
                    const validationSpan = item.querySelector(`[data-valmsg-for="${originalName}"]`);
                    if (validationSpan) {
                        validationSpan.setAttribute('data-valmsg-for', newName);
                    }
                });
                // Special handling for hidden fields that might be named differently
                // (e.g., EventPhotos.Index for jQuery Unobtrusive)
                const hiddenIndexInput = item.querySelector('input[type="hidden"][name="EventPhotos.Index"]');
                if (hiddenIndexInput) {
                    hiddenIndexInput.value = index;
                }
            });
        }

        // Add New Photo Button Click Handler
        addPhotoBtn.addEventListener('click', function () {
            // HTML template for a new photo item
            const newPhotoHtml = `
                <div class="card card-dashed card-xl-stretch mb-xl-8 photo-item">
                    <div class="card-body">
                        <div class="d-flex align-items-center mb-5">
                            <div class="flex-grow-1">
                                <h4 class="mb-0">Photo #<span class="photo-number">${photoIndex + 1}</span></h4>
                            </div>
                            <button type="button" class="btn btn-sm btn-icon btn-light-danger remove-photo-btn">
                                <i class="ki-outline ki-cross fs-2"></i>
                            </button>
                        </div>
                        <input type="hidden" name="EventPhotos.Index" value="${photoIndex}" />
                        <input type="hidden" name="EventPhotos[${photoIndex}].Id" value="0" />
                        <input type="hidden" name="EventPhotos[${photoIndex}].IsDeleted" class="is-deleted-input" value="false" />
                        <div class="mb-5">
                            <label class="form-label fw-bold">Image File</label>
                            <input type="file" name="EventPhotos[${photoIndex}].PhotoFile" class="form-control form-control-solid" accept="image/*" required />
                            <span class="text-danger" data-valmsg-for="EventPhotos[${photoIndex}].PhotoFile"></span>
                        </div>
                        <div class="mb-5">
                            <label class="form-label fw-bold">Description</label>
                            <textarea name="EventPhotos[${photoIndex}].Description" class="form-control form-control-solid" rows="2" placeholder="Enter photo description"></textarea>
                            <span class="text-danger" data-valmsg-for="EventPhotos[${photoIndex}].Description"></span>
                        </div>
                        <div class="mb-5">
                            <label class="form-label fw-bold">Order</label>
                            <input type="number" name="EventPhotos[${photoIndex}].Order" class="form-control form-control-solid" value="0" />
                            <span class="text-danger" data-valmsg-for="EventPhotos[${photoIndex}].Order"></span>
                        </div>
                    </div>
                </div>
            `;
            photosContainer.insertAdjacentHTML('beforeend', newPhotoHtml); // Add the new HTML to the container
            photoIndex++; // Increment index for the next new photo
            updatePhotoNumbers(); // Re-index all photos (including the new one)
            rebindValidation(); // Rebind jQuery Unobtrusive Validation to include new inputs
        });

        // Event delegation for Remove Photo buttons (handles both existing and dynamically added photos)
        photosContainer.addEventListener('click', function (event) {
            if (event.target.closest('.remove-photo-btn')) { // Check if the clicked element or its parent is a remove button
                const button = event.target.closest('.remove-photo-btn');
                const photoItem = button.closest('.photo-item'); // Get the parent photo item div
                const photoId = photoItem.dataset.photoId; // Get data-photo-id attribute (will be "0" for new photos)

                // Use SweetAlert2 for confirmation
                if (typeof Swal !== 'undefined') {
                    Swal.fire({
                        title: 'Are you sure?',
                        text: "You are about to remove this photo. This action cannot be undone.",
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#d33',
                        cancelButtonColor: '#3085d6',
                        confirmButtonText: 'Yes, remove it!'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            if (photoId && parseInt(photoId) > 0) {
                                // If it's an existing photo (has a positive ID), mark it for deletion
                                const isDeletedInput = photoItem.querySelector('.is-deleted-input');
                                if (isDeletedInput) {
                                    isDeletedInput.value = 'true';
                                }
                                photoItem.style.display = 'none'; // Hide it visually
                            } else {
                                // If it's a newly added photo (ID is 0 or not set), just remove it from the DOM
                                photoItem.remove();
                            }
                            updatePhotoNumbers(); // Re-index and update display for remaining photos
                            if (typeof toastr !== 'undefined') {
                                toastr.success('Photo removed successfully.');
                            }
                            rebindValidation(); // Rebind validation
                        }
                    });
                } else {
                    // Fallback to native confirm if SweetAlert2 is not available
                    if (confirm('Are you sure you want to remove this photo?')) {
                        if (photoId && parseInt(photoId) > 0) {
                            const isDeletedInput = photoItem.querySelector('.is-deleted-input');
                            if (isDeletedInput) {
                                isDeletedInput.value = 'true';
                            }
                            photoItem.style.display = 'none';
                        } else {
                            photoItem.remove();
                        }
                        updatePhotoNumbers();
                        rebindValidation();
                    }
                }
            });
    });

// Handle file input change for immediate image preview
photosContainer.addEventListener('change', function (event) {
    // Check if the changed element is a file input with the specified class
    if (event.target.matches('input[type="file"].new-file-input')) {
        const file = event.target.files[0]; // Get the selected file
        if (file) {
            const reader = new FileReader(); // Create a FileReader instance
            // Find the image element within the current photo item to update its src
            const imgElement = event.target.closest('.photo-item').querySelector('.current-photo-preview, .new-photo-preview');
            if (imgElement) {
                reader.onload = function (e) {
                    imgElement.src = e.target.result; // Set the image source to the FileReader result
                };
                reader.readAsDataURL(file); // Read the file as a Data URL
            }
        } else {
            // If no file is selected (e.g., cleared the input)
            const imgElement = event.target.closest('.photo-item').querySelector('.current-photo-preview, .new-photo-preview');
            if (imgElement) {
                // Revert to original image if it was an existing photo with an original source
                if (imgElement.dataset.originalSrc) {
                    imgElement.src = imgElement.dataset.originalSrc;
                } else {
                    // Otherwise, revert to a generic placeholder for new photos
                    imgElement.src = 'https://placehold.co/150x150/e0e0e0/ffffff?text=New+Photo'; // Using placeholder
                }
            }
        }
    }
});

// Function to rebind jQuery Unobtrusive Validation after dynamic DOM changes
function rebindValidation() {
    // Ensure jQuery and validation scripts are loaded before attempting to rebind
    if (typeof $ !== 'undefined' && $.validator && $.validator.unobtrusive) {
        // Remove existing validation data from the form
        $('form#event-create-form').removeData('validator');
        $('form#event-create-form').removeData('unobtrusiveValidation');
        // Re-parse the form to apply validation rules to newly added/modified elements
        $.validator.unobtrusive.parse('form#event-create-form');
    }
}

// Initial update of photo numbers when the page loads (for cases where model.EventPhotos already exist)
updatePhotoNumbers();
    }


// --- Global Select2 Initialization ---
// Ensure jQuery and Select2 are loaded before trying to initialize Select2
if (typeof $ !== 'undefined' && $.fn.select2) {
    $('.form-select[data-control="select2"]').select2({
        theme: "bootstrap-5", // Assuming you're using the Bootstrap 5 theme for Select2
        width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
        placeholder: $(this).data('placeholder'),
        allowClear: Boolean($(this).data('allow-clear')),
    });
}

    // --- Theme-Specific JavaScript Initialization (if any) ---
    // If your Metronic/KeenThemes theme requires specific JS initialization functions
    // (e.g., for dropdowns, sidebars, modals), you might call them here.
    // Example: KTMenu.init(); KTApp.init(); KTUtil.init();
    // This is highly dependent on your specific theme setup.
});
