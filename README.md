jQuery-tmpl.NET
===============

A simple .NET library for rendering jQuery templates server-side.


Version History
---------------

* v0.2: Arbitrary function evaluation, nested templates with {{tmpl}}, 
        and custom tag parameters for {{each}}

* v0.1: Basic functionality in place


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
data object. Nested property resolution is supported, thanks to shellscape!
Note that expression/function evaluation is not supported.

* **`{{= property}}`**: Handled identically to `${}`.

* **`{{each property}}...{{/each}}`**: Renders an instance of the tag 
contents for each item in the property value on the provided data object. 
The property must be an array of objects, and custom index or value
variables are supported via the `{{each(index,value) data}}` syntax.

* **`{{if property}}...{{/if}`**: Renders the content of the tag if the 
property value on the provided data object evaluates to `true`. This is
javascript-style evaluation so 0, null, empty string are all `false`.

* **`{{else property}}`**: Used within the `{{if}}` tag to evaluate else
conditions. The property value is optional.

* **`{{html property}}`**: Renders the value of the property without HTML
encoding. Otherwise identical to `${}`.

* **`{{tmpl "#templateName"}}`**: Used for template composition. A named
template is evaluated with the current data item within the parent template.
The named template must first be registered with the `TemplateEngine`
class before it can be used.


Roadmap
-------

* Better APIs for nested template evaluation.

* Pluggable template cache.

* {{wrap}} implementation.

* Whatever [garann](http://github.com/garann) asks me for