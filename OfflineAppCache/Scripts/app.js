window.App = window.App || {
    templates: {}
};

App.init = function() {
    var source = $("#shipTemplate").html();
    App.templates.ship = Handlebars.compile(source);

    $('#singleShipButton').click(function() {
        $.ajax({
            url: '/Home/RandomShip',
            type: 'GET',
            dataType: 'json',
            timeout: 2000,
            success: function(response) {
                console.log(response);
                var output = App.templates.ship(response);
                $('#results').html(output);
            },
            error: function(x, t, m) {
                if (t === 'timeout') {
                    alert('request received no response. you may be offline');
                } else {
                    alert(t);
                }}});
    });
};