jQuery-tmpl.NET
===============

A simple .NET library for rendering jQuery templates server-side.

Usage
-----

The API is written to be a simple as possible. Given an html fragment
with embedded jQuery templates `tmpl` and a .NET object `data`:

    new TemplateEngine().Render(tmpl, data);

This renders the template in (hopefully) much the same way that 
jquery-tmpl would do on the client side.

Additional support is provided for ASP.NET MVC with the jQueryTmpl.Mvc
library. This contains an `HtmlHelper` extension method that can be used
directly on views to render partial views containing jQuery templates:

    <%= Html.RenderTemplate("PartialViewName", model) %>


Supported Tags
--------------

* **`${property}`**: Both the shorthand and `{{= }}` are supported. 
Will print out the value of the indicated property on the provided
data object. Note that expression/function evaluation is not supported.

* **`{{= property}}`**: Handled identically to `${}`.

* **`{{each property}}...{{/each}}`**: Renders an instance of the tag 
contents for each item in the property value on the provided data object. 
Currently, the property must be an array of objects and does not support 
custom index or value variables. Properties of the objects in the array 
can be evaluated within the block using any of the tags, however.

* **`{{if property}}...{{/if}`**: Renders the content of the tag if the 
property value on the provided data object evaluates to `true`. Will throw 
an exception if the property cannot be parsed into a boolean.

* **`{{else property}}`**: Used within the `{{if}}` tag to evaluate else
conditions. The property value is optional, but if present must be parseable
into a boolean.

* **`{{html property}}`**: Renders the value of the property without HTML
encoding. Otherwise identical to `${}`.


Roadmap
-------

* Support nested template rendering using `{{tmpl}}` and exposing a method
on the `TemplateEngine` class to register named templates before rendering.

* Support custom index/value variables on the `{{each}}` tag.

* Whatever [gmeans](http://github.com/gmeans) asks me for