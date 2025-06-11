"use strict";
var KTSigninGeneral = function () {
    var t, e, i;
    return {
        init: function () {
            t = document.querySelector("#kt_sign_in_form"),
                e = document.querySelector("#kt_sign_in_submit"),
                i = FormValidation.formValidation(t, {
                    fields: {
                        Email: { // Changed to 'Email' to match asp-for
                            validators: {
                                notEmpty: {
                                    message: "Email address is required"
                                },
                                emailAddress: {
                                    message: "The value is not a valid email address"
                                }
                            }
                        },
                        Password: { // Changed to 'Password' to match asp-for
                            validators: {
                                notEmpty: {
                                    message: "The password is required"
                                }
                            }
                        }
                    },
                    plugins: {
                        trigger: new FormValidation.plugins.Trigger,
                        bootstrap: new FormValidation.plugins.Bootstrap5({
                            rowSelector: ".fv-row"
                        })
                    }
                }),
                e.addEventListener("click", (function (n) {
                    n.preventDefault(), // PREVENT DEFAULT FORM SUBMISSION HERE!
                        i.validate().then((function (validationStatus) { // Renamed 'i' to 'validationStatus' for clarity
                            if (validationStatus == "Valid") {
                                // Client-side validation passed, now submit via AJAX
                                e.setAttribute("data-kt-indicator", "on"); // Show loading indicator
                                e.disabled = true; // Disable button

                                const formData = new FormData(t); // Get all form data, including inputs and anti-forgery token

                                // Get the form's action URL and method
                                const formAction = t.action;
                                const formMethod = t.method;

                                fetch(formAction, {
                                    method: formMethod,
                                    body: formData,
                                    // If your server returns JSON, ensure the Accept header is set
                                    headers: {
                                        'Accept': 'application/json'
                                    }
                                })
                                    .then(response => {
                                        // Check if the response was successful (e.g., 200 OK)
                                        if (!response.ok) {
                                            // If not successful, try to read error message or throw error
                                            return response.json().then(errorData => {
                                                throw new Error(errorData.message || "Server error occurred.");
                                            });
                                        }
                                        return response.json(); // Parse the JSON response from the server
                                    })
                                    .then(data => {
                                        e.removeAttribute("data-kt-indicator"); // Hide loading indicator
                                        e.disabled = false; // Re-enable button

                                        if (data.success) { // Assuming server sends { success: true } on successful login
                                            Swal.fire({
                                                text: data.message || "You have successfully logged in!",
                                                icon: "success",
                                                buttonsStyling: !1,
                                                confirmButtonText: "Ok, got it!",
                                                customClass: {
                                                    confirmButton: "btn btn-primary"
                                                }
                                            }).then((function (result) {
                                                if (result.isConfirmed) {
                                                    // Clear form fields
                                                    t.querySelector('[name="Email"]').value = "";
                                                    t.querySelector('[name="Password"]').value = "";
                                                    // Redirect to dashboard or home page if success and redirectUrl is provided
                                                    if (data.redirectUrl) {
                                                        window.location.href = data.redirectUrl;
                                                    }
                                                }
                                            }));
                                        } else {
                                            // Login failed (e.g., invalid credentials)
                                            Swal.fire({
                                                text: data.message || "Invalid login attempt. Please check your credentials.",
                                                icon: "error",
                                                buttonsStyling: !1,
                                                confirmButtonText: "Ok, got it!",
                                                customClass: {
                                                    confirmButton: "btn btn-primary"
                                                }
                                            });
                                        }
                                    })
                                    .catch(error => {
                                        // Catch any network errors or errors thrown in .then() block
                                        console.error('AJAX Error:', error);
                                        e.removeAttribute("data-kt-indicator");
                                        e.disabled = false;
                                        Swal.fire({
                                            text: error.message || "An unexpected error occurred during login. Please try again.",
                                            icon: "error",
                                            buttonsStyling: !1,
                                            confirmButtonText: "Ok, got it!",
                                            customClass: {
                                                confirmButton: "btn btn-primary"
                                            }
                                        });
                                    });

                            } else {
                                // Client-side validation failed
                                Swal.fire({
                                    text: "Sorry, looks like there are some errors detected, please try again.",
                                    icon: "error",
                                    buttonsStyling: !1,
                                    confirmButtonText: "Ok, got it!",
                                    customClass: {
                                        confirmButton: "btn btn-primary"
                                    }
                                });
                            }
                        }))
                }))
        }
    }
}();
KTUtil.onDOMContentLoaded((function () {
    KTSigninGeneral.init()
}));