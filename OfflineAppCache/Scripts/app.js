
(function (app, $, undefined) {
    var templates = {};

    function initHandlebars() {
        var source = $("#shipTemplate").html();
        templates.ship = Handlebars.compile(source);
    }

    app.init = function () {
        initHandlebars();

        $('#singleShipButton').click(function () {
            $.ajax({
                url: '/Home/RandomShip',
                type: 'GET',
                dataType: 'json',
                timeout: 2000,
                success: function (response) {
                    console.log(response);
                    var output = templates.ship(response);
                    $('#results').html(output);
                },
                error: function (x, t) {
                    if (t === 'timeout') {
                        alert('request received no response. you may be offline');
                    } else {
                        console.log(x);
                        alert(t);
                    }
                }
            });
        });
    };
}(window.app = window.app || {}, jQuery));

