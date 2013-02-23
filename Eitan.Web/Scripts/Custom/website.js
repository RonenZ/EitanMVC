$(document).ready(function () {

    if (!(isRunningIE10 || isRunningIE8OrBelow)) {
        $('#render-body img').hide().load(function () {
            $(this).fadeIn(500);
        });
    }

    if ($('.portfolio-item-hover-content').length && jQuery()) {
            function hover_effect() {
                $('.portfolio-item-hover-content').live({
                    mouseenter:
                    function () {
                        $(this).find('div,a').stop(0, 0).removeAttr('style');
                        $(this).find('.hover-options').animate({ opacity: 0.5 }, 'fast');
                        $(this).find('.portfolio-content-wrap').animate({ "top": "35%" }, 400);
                    }, mouseleave:
                    function () {
                        $(this).find('.hover-options').stop(0, 0).animate({ opacity: 0 }, "fast");
                        $(this).find('a').stop(0, 0).animate({ "top": "150%" }, "slow");
                        $(this).find('.portfolio-content-wrap').stop(0, 0).animate({ "top": "150%" }, "slow");
                    }
                });
            }
        }
        hover_effect();
});