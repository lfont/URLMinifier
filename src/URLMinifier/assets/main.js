(function (exports, qwery, bonzo) {
    'use strict';

    exports.dom = function (selector, context) {
        return bonzo(qwery(selector, context));
    };

    exports.dom.create = bonzo.create;
}(window.URLMinifier = {}, qwery, bonzo));
