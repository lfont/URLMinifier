(function (exports) {
    'use strict';

    exports.errorView = function (dom, selector) {
        var $el = dom(selector);

        return {
            hide: function () {
                $el.empty()
                  .hide();
            },

            show: function (message) {
                $el.text(message)
                  .show();
            }
        };
    };

    exports.outputView = function (dom, selector, event, window) {
        var $el       = dom(selector),
            $titleEl  = dom('.output-view-title', $el),
            $itemsEl  = dom('.output-view-items', $el),
            template  = dom('.output-view-item-template', $el).text();

        return {
            append: function (longUrl, shortUrl) {
                var $itemEl     = dom(dom.create(template)[0]),
                    $shortUrlEl = dom('input', $itemEl),
                    $longUrlEl  = dom('a', $itemEl);

                $titleEl.show();

                $shortUrlEl.val(shortUrl);

                $longUrlEl
                    .attr('href', longUrl)
                    .text(longUrl);

                $itemsEl.prepend($itemEl);

                var shortUrlEl = $shortUrlEl[0];

                event.on(shortUrlEl, 'keypress', function onKeypress (e) {
                    if (e.which === 13) {
                        window.open(shortUrl, '_blank');
                    }
                });

                shortUrlEl.select();
                $shortUrlEl.focus();
            }
        };
    };

    exports.formView = function (dom, selector, event, http, errorView, outputView) {
        var $el      = dom(selector),
            $inputEl = dom('input', $el);

        var view = {
            sendMinifyRequest: function (action, method, name, value) {
                errorView.hide();
                var data = {};
                data[name] = value;
                return http[method](action, data)
                    .then(function onMinifyResponse (response) {
                        outputView.append(value, response);
                    })
                    .catch(function onMinifyError (message) {
                        if (this.status === 400) {
                            errorView.show('The given value is not a valid url.');
                        } else {
                            errorView.show('Oops an error has occur.');
                        }
                    });
            },

            submitValue: function () {
                $inputEl.removeClass('view-input-error');

                this.sendMinifyRequest($el.attr('action'), $el.attr('method'),
                                       $inputEl.attr('name'), $inputEl.val())
                    .then(function onRequestSuccess () {
                        $inputEl.val('');
                    })
                    .catch(function onRequestError () {
                        $inputEl
                            .addClass('view-input-error')
                            .focus();

                        event.one($inputEl[0], 'keydown', function onKeydown () {
                            $inputEl.removeClass('view-input-error');
                            errorView.hide();
                        });
                    });
            },

            initialize: function () {
                $inputEl.focus();

                event.on($el[0], 'submit', function onFormSubmit (e) {
                    e.preventDefault();
                    view.submitValue();
                });
            }
        };

        return view;
    };
}(window.URLMinifier.minifier = {}));
