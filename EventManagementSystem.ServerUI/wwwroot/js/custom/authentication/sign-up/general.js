"use strict";
var KTSignupGeneral = function () {
    var e, t, a, s, r = function () { return 100 === s.getScore() };

    return {
        init: function () {
            e = document.querySelector("#kt_sign_up_form"), // Your form element
                t = document.querySelector("#kt_sign_up_submit"), // Your submit button
                s = KTPasswordMeter.getInstance(e.querySelector('[data-kt-password-meter="true"]')),
                a = FormValidation.formValidation(e, {
                    fields: {
                        // !!! IMPORTANT: Adjust these field names to match your *rendered HTML* name attributes.
                        // If your asp-for generates name="FirstName", change "first-name" to "FirstName"
                        "FirstName": { // Example: If your model property is FirstName
                            validators: {
                                notEmpty: { message: "First Name is required" }
                            }
                        },
                        "LastName": { // Example: If your model property is LastName
                            validators: {
                                notEmpty: { message: "Last Name is required" }
                            }
                        },
                        "Email": { // Example: If your model property is Email
                            validators: {
                                notEmpty: { message: "Email address is required" },
                                emailAddress: { message: "The value is not a valid email address" }
                            }
                        },
                        "Password": { // Example: If your model property is Password
                            validators: {
                                notEmpty: { message: "The password is required" },
                                callback: { message: "Please enter valid password", callback: function (e) { if (e.value.length > 0) return r() } }
                            }
                        },
                        "ConfirmPassword": { // Example: If your model property is ConfirmPassword
                            validators: {
                                notEmpty: { message: "The password confirmation is required" },
                                identical: {
                                    compare: function () { return e.querySelector('[name="Password"]').value }, // Make sure this also matches casing
                                    message: "The password and its confirm are not the same"
                                }
                            }
                        },
                        "Toc": { // Example: If your model property is Toc (Terms and Conditions)
                            validators: {
                                notEmpty: { message: "You must accept the terms and conditions" }
                            }
                        }
                    },
                    plugins: {
                        trigger: new FormValidation.plugins.Trigger({ event: { password: !1 } }),
                        bootstrap: new FormValidation.plugins.Bootstrap5({ rowSelector: ".fv-row", eleInvalidClass: "", eleValidClass: "" })
                    }
                }),
                t.addEventListener("click", (function (n) { // Renamed 'r' to 'n' to avoid conflict with 'r' function
                    n.preventDefault(), // Prevent default form submission
                        a.revalidateField("Password"), // Revalidate password field
                        a.validate().then((function (validationStatus) { // Renamed 'a' to 'validationStatus'
                            if (validationStatus == "Valid") {
                                // Client-side validation passed, now submit via AJAX
                                t.setAttribute("data-kt-indicator", "on"); // Show loading indicator
                                t.disabled = true; // Disable button

                                const formData = new FormData(e); // Get all form data from the form element 'e'

                                // Get the form's action URL and method
                                const formAction = e.action;
                                const formMethod = e.method;

                                fetch(formAction, {
                                    method: formMethod,
                                    body: formData,
                                    headers: {
                                        'Accept': 'application/json'
                                    }
                                })
                                    .then(response => {
                                        if (!response.ok) {
                                            return response.json().then(errorData => {
                                                throw new Error(errorData.message || "Server error occurred.");
                                            });
                                        }
                                        return response.json(); // Parse the JSON response from the server
                                    })
                                    .then(data => {
                                        t.removeAttribute("data-kt-indicator"); // Hide loading indicator
                                        t.disabled = false; // Re-enable button

                                        if (data.success) { // Assuming server sends { success: true } on successful signup
                                            Swal.fire({
                                                text: data.message || "You have successfully signed up!", // Updated success message
                                                icon: "success",
                                                buttonsStyling: !1,
                                                confirmButtonText: "Ok, got it!",
                                                customClass: { confirmButton: "btn btn-primary" }
                                            }).then((function (result) {
                                                if (result.isConfirmed) {
                                                    e.reset(); // Reset the form
                                                    s.reset(); // Reset the password meter
                                                    if (data.redirectUrl) {
                                                        window.location.href = data.redirectUrl; // Redirect if provided
                                                    }
                                                }
                                            }));
                                        } else {
                                            // Signup failed (e.g., email already exists, server-side validation error)
                                            Swal.fire({
                                                text: data.message || "Sorry, looks like there are some errors detected, please try again.",
                                                icon: "error",
                                                buttonsStyling: !1,
                                                confirmButtonText: "Ok, got it!",
                                                customClass: { confirmButton: "btn btn-primary" }
                                            });
                                        }
                                    })
                                    .catch(error => {
                                        console.error('AJAX Error:', error);
                                        t.removeAttribute("data-kt-indicator");
                                        t.disabled = false;
                                        Swal.fire({
                                            text: error.message || "An unexpected error occurred during signup. Please try again.",
                                            icon: "error",
                                            buttonsStyling: !1,
                                            confirmButtonText: "Ok, got it!",
                                            customClass: { confirmButton: "btn btn-primary" }
                                        });
                                    });

                            } else {
                                // Client-side validation failed
                                Swal.fire({
                                    text: "Sorry, looks like there are some errors detected, please try again.",
                                    icon: "error",
                                    buttonsStyling: !1,
                                    confirmButtonText: "Ok, got it!",
                                    customClass: { confirmButton: "btn btn-primary" }
                                });
                            }
                        }))
                })),
                e.querySelector('input[name="Password"]').addEventListener("input", (function () { // Changed 'password' to 'Password'
                    this.value.length > 0 && a.updateFieldStatus("Password", "NotValidated") // Changed 'password' to 'Password'
                }))
        }
    }
}();

KTUtil.onDOMContentLoaded((function () {
    KTSignupGeneral.init()
}));