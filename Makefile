BUILD ?= Debug

SRC = src/URLMinifier
DEST = build/URLMinifier/bin/$(BUILD)

JS = node_modules/qwery/qwery.min.js \
     node_modules/bonzo/bonzo.min.js \
     node_modules/bean/bean.min.js \
     node_modules/qwest/qwest.min.js \
     $(SRC)/assets/main.js

JS_MINIFY = $(SRC)/Minify/assets/minifier.js \
            $(SRC)/Minify/assets/main.js

SRC_TEST = src/URLMinifier.Test
DEST_TEST = build/URLMinifier.Test/bin/Debug/

all: $(DEST)/url-minifier.exe $(DEST_TEST)/url-minifier-test.exe

packages:
	mono vendor/NuGet.exe install packages.config -OutputDirectory packages

$(DEST)/url-minifier.exe: packages
	xbuild /property:Configuration=$(BUILD) $(SRC)/URLMinifier.fsproj

$(DEST)/Content/main.js: $(JS)
	mkdir -p $(DEST)/Content
	touch $(DEST)/Content/main.js
	for f in $^; do (cat $$f; echo ';') >> $@; done

$(DEST)/Content/Minify/main.js: $(JS_MINIFY)
	mkdir -p $(DEST)/Content/Minify
	cat > $@ $^

node_modules:
	npm install

assets: node_modules $(DEST)/Content/main.js $(DEST)/Content/Minify/main.js
	cp $(SRC)/Minify/assets/index.css $(DEST)/Content/Minify/

run: $(DEST)/url-minifier.exe assets
	mono $(DEST)/url-minifier.exe

$(DEST_TEST)/url-minifier-test.exe: packages
	xbuild $(SRC_TEST)/URLMinifier.Test.fsproj

test: $(DEST_TEST)/url-minifier-test.exe
	mono $(DEST_TEST)/url-minifier-test.exe

clean:
	rm -rf build

.PHONY: $(DEST)/Content/Minify/main.js
