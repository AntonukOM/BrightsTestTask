(function () {
    var onParseClick = function () {
        var list = $('#urlList').val();
        var res = $('#result');
        res.empty();

        if (!list) {
            res.append("<h3 class='text-danger'>No data found</h3>");
            return;
        }

        $.ajax({
            type: 'get',
            url: '/Parse/GetTitleList',
            dataType: 'json',
            data: {
                urlList : list
            }
        }).done(function (data) {
            //console.log('Parsed with success ' + data.length + ' urls');
            //for (var i = 0; i < data.length; ++i) {
            //    console.log('Title = ' + data[i].Title + ' Url = ' + data[i].Url);
            //}
            var res = $('#result');

            if (data.length > 0) {
                var endStr = 's';
                if (data.length == 1) {
                    endStr = '';
                }
                res.append("<h3 class='text-success'> Parsed with success " + data.length + " item"  + endStr + "</h3>");
                res.append("<ul class='list-group'>");
                for (var i = 0; i < data.length; ++i) {
                    res.append("<li class='list-group-item'>" + data[i].Url + " - " + data[i].Title + "</li>");
                }
                res.append("</ul>");
            } else {
                res.append("<h3 class='text-warning'>Failed to parse current data</h3>");
                res.append("<p>Try to enter data in the correct format. For example: <span class='text-primary'>https://www.google.com.ua</span>.</p>");
                res.append("<p>And use delemeters (<span class='text-primary'>space, comma, semicolon, carriage</span>) after url inputs. </p>");
            }

        }).fail(function () {
            console.log('ajax FAIL');
            res.append("<h3 class='text-danger'>Opps! Server error!! Sorry:(</h3>");
        });
    };

    var onClearClick = function() {
        $('#result').empty();
    };

    var initParseBtnClick = function () {
        $('#parse').on('click', onParseClick);
        //console.log('Click init is fine');
    };

    var initClearBtnClick = function () {
        $('#clear').on('click', onClearClick);
    };
    
    $(function () {
        initParseBtnClick();
        initClearBtnClick();
        //console.log('index.js loaded');
    });    
})();