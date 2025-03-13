
document.addEventListener("DOMContentLoaded", function () {
    attachTooltipEvents();
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
        attachTooltipEvents();
    });

    // Function to attach event listeners
    function attachTooltipEvents() {
        document.querySelectorAll(".block").forEach(block => {
            block.addEventListener("click", function (event) {
                // Remove active class from all blocks
                document.querySelectorAll(".block").forEach(el => el.classList.remove("active"));

                // Add active class only to the clicked block
                this.classList.add("active");

                let tooltip = document.querySelector(".tooltip");
                if (tooltip) {
                    adjustTooltipPosition(this, tooltip);
                }

                // Stop event propagation to prevent closing immediately
                event.stopPropagation();
            });
        });

        // Hide tooltip when clicking anywhere else
        document.addEventListener("click", function () {
            document.querySelectorAll(".block").forEach(el => el.classList.remove("active"));
//            document.querySelectorAll(".tooltip").forEach(el => el.classList.remove("activeLeft"));
        });
    }

    // Function to check and adjust tooltip position
    function adjustTooltipPosition(block, tooltip) {

        let blockRect = block.getBoundingClientRect();
        let tooltipRect = tooltip.getBoundingClientRect();
        let screenWidth = window.innerWidth;

        console.log(blockRect);

        let xmid = (blockRect.left + blockRect.right) / 2;
        let tw2 = tooltipRect.width / 2;

        // If tooltip overflows, align it properly
        if (xmid + tw2 > screenWidth) {
            console.log("OVERFLOW");
            tooltip.classList.add("activeLeft");
            // tooltip.style.left = "auto";
            // tooltip.style.right = "0";
            tooltip.style.transform = "translateX(-70%)"; // Align to right
        } else {
            tooltip.style.left = "50%";
            tooltip.style.right = "auto";
            tooltip.style.transform = "translateX(-50%)"; // Center it
        }
        console.log(window.getComputedStyle(tooltip).transform);
    }
});
