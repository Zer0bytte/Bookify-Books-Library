$(document).ready(function () {

    var books = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: '/Search/Find?query=%QUERY',
            wildcard: '%QUERY'
        }
    });

    $('#Search').typeahead({
        minLength: 4,
        highlight: true
    }, {
        name: 'book',
        display: 'title',
        limit: 100,
        source: books,
        templates: {
            empty: [
                '<div class="m-4 fw-bold">',
                'No books where found!',
                '</div>'
            ].join('\n'),
            suggestion: Handlebars.compile
                (`
                <div class="py-2">
                    <span>{{title}}</span></br>
                    <span class="f-xs text-gray-400">by {{author}}</span>
                </div>`)
        }

    }).on('typeahead:select', function (e, book) {
        window.location.replace(`/Search/Details?id=${book.key}`);
    });




});