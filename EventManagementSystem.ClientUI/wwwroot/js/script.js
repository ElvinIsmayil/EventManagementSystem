function showStep(stepNumber) {
            // Hide all steps
            document.querySelectorAll('.booking-step').forEach(step => {
                step.classList.add('hidden');
            });
            
            // Show the selected step
            document.getElementById('step' + stepNumber).classList.remove('hidden');
        }
        
        // Seat selection functionality
        document.addEventListener('DOMContentLoaded', function() {
            const seats = document.querySelectorAll('.seat:not(.seat-taken)');
            seats.forEach(seat => {
                seat.addEventListener('click', function() {
                    if (!this.classList.contains('seat-taken')) {
                        this.classList.toggle('seat-selected');
                    }
                });
            });
        });