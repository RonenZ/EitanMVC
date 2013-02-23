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


    var History = window.History, // Note: We are using a capital H instead of a lower h
            State = History.getState(),
            $log = $('#log');

    ko.applyBindings(ViewModel);

    $("#releases-div").isotope({
        layoutMode: 'fitRows',
        itemSelector: '.isotope-item'
    });

    $("#projects-div").isotope({
        layoutMode: 'fitRows',
        itemSelector: '.isotope-item'
    });

    $("#news-div").isotope({
        layoutMode: 'fitRows',
        animationOptions: {
            duration: 750,
            easing: 'easeInOutBack',
            queue: false
        },
        itemSelector: '.isotope-item'
    });

    $(".btn-more").click(function () {

        var name = $(this).data("name");
        var path = $(this).data("path");
        var page = $(this).data("page");
        page++;
        var data = "?json=true&page=" + page;
        var Objects = eval("ViewModel." + name);

        $(this).data("page", page);

        if (name == "News")
            GetFromWebApiDetails(ViewModel.News, "News", data, ".btn-more-News");
        else
            GetFromWebApi(Objects, path, data, ".btn-more-" + name);

        $('html, body').animate({ scrollTop: $(document).height() }, 'slow');
    });


    History.Adapter.bind(window, 'statechange', function () { // Note: We are using statechange instead of popstate
        // Log the State
        var State = History.getState(); // Note: We are using History.getState() instead of event.state
        if (State.data.nav != null) {
            var type = State.data.nav;

            $("#render-body").hide();
            reset_isotope(type);
            eval("ViewModel.Load" + type + "();");
        }
        else
            window.location = "/";
    });

    $(".AjaxLink").click(function () {
        var type = $(this).data("type");
        var toactive = $(this).parent();

        //activate li of link
        activate_menu_li(toactive);


        $("#render-body").hide();

        History.pushState({ state: type, nav: type }, type, "/" + type + "/");

        return false;

    });

    $(".release-filter-bar").hover(function () {
        $(this).animate({ height: '100px' });
    }, function () {
        $(this).animate({ height: '30px' });
    });
    

    $('.release-browse-span').live('click', function () {
        var selector = $(this).data('value');
        var $container = $('.releases-div');
        $container.isotope({
            filter: selector,
            animationOptions: {
                duration: 750,
                easing: 'linear',
                queue: false
            }
        });
        return false;
    });
});


function activate_menu_li(toactive) {
    $(".menu-links li").removeClass("active");
    $(toactive).addClass("active");
}

function reset_isotope(colname) {
}