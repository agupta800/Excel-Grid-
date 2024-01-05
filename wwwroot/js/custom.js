jQuery(function ($) {
  $(".sidebar-dropdown > a").click(function () {
    $(".sidebar-submenu").slideUp(200);
    if ($(this).parent().hasClass("active")) {
      $(".sidebar-dropdown").removeClass("active");
      $(this).parent().removeClass("active");
    } else {
      $(".sidebar-dropdown").removeClass("active");
      $(this).next(".sidebar-submenu").slideDown(200);
      $(this).parent().addClass("active");
    }
  });

  $(document).ready(function () {
    function checkScreenSize() {
      var screenWidth = $(window).width();
      var pageWrapper = $(".page-wrapper");

      if (screenWidth <= 991) {
        pageWrapper.removeClass("toggled");
      } else {
        pageWrapper.addClass("toggled");
      }
    }
    // Function to handle the resizing of the grid
    function resizeGrid() {
      var screenWidth = $(window).width();
      var gridElement1 = $(".k-form-fieldset .k-form-layout");

      // Check if the screen width is less than 500 pixels
      if (screenWidth <= 600) {
        gridElement1.addClass("k-grid-cols-1");
        gridElement1.removeClass("k-grid-cols-2");
        gridElement1.removeClass("k-grid-cols-3");
        gridElement1.removeClass("k-grid-cols-4");
      } else if (screenWidth < 1000 && screenWidth > 500) {
        gridElement1.addClass("k-grid-cols-2");
        gridElement1.removeClass("k-grid-cols-3");
        gridElement1.removeClass("k-grid-cols-1");
        gridElement1.removeClass("k-grid-cols-4");
      } else if (screenWidth < 2000 && screenWidth > 1000) {
        gridElement1.addClass("k-grid-cols-3");
        gridElement1.removeClass("k-grid-cols-4");
        gridElement1.removeClass("k-grid-cols-2");
        gridElement1.removeClass("k-grid-cols-1");
      } else if (screenWidth > 2000) {
        gridElement1.addClass("k-grid-cols-4");
        gridElement1.removeClass("k-grid-cols-3");
        gridElement1.removeClass("k-grid-cols-2");
        gridElement1.removeClass("k-grid-cols-1");
      } else {
        gridElement1.addClass("k-grid-cols-1");
        gridElement1.removeClass("k-grid-cols-2");
        gridElement1.removeClass("k-grid-cols-3");
        gridElement1.removeClass("k-grid-cols-4");
      }
    }
    // Call the function on page load and window resize
    $(window).on("load resize", function () {
      checkScreenSize();
      resizeGrid();
    });
    $("#show-sidebar").click(function () {
      $(".page-wrapper").toggleClass("toggled");
    });

    // Initial call to resize the grid based on the current screen size
    resizeGrid();
    // Resize the grid on window resize
    $(window).resize(function () {
      resizeGrid();
    });
  });
});

$(document).ready(function () {
  $(".nav-treeview > .nav-item > .nav-link").click(function (event) {
    var navItemParent = $(this)
      .closest(".nav-item-parent")
      .next(".nav-item-parent");

    // Save state in local storage
    localStorage.setItem("menuState", "menu-open");
  });

  // Retrieve and apply the saved state on page load
  var savedState = localStorage.getItem("menuState");
  if (savedState === "menu-open") {
    $(".nav-item-parent").each(function () {
      if ($(this).find(".active").length > 0) {
        $(this).addClass("menu-open");
      } else {
        $(this).removeClass("menu-open");
      }
    });
  }
});
