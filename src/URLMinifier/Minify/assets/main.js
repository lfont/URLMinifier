(function (window, bean, qwest, dom, minifier) {
    'use strict';

    bean.on(window, 'load', function () {
        var errorView  = minifier.errorView(dom, '#errorView'),
            outputView = minifier.outputView(dom, '#outputView', bean, window),
            formView   = minifier.formView(dom, '#formView', bean, qwest, errorView, outputView);

        formView.initialize();
    });
}(window, bean, qwest, URLMinifier.dom, URLMinifier.minifier));
