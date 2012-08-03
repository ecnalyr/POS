$(document).ready(function () {
    /***********/
    securityToken = $('[name=__RequestVerificationToken]').val();
    $('body').bind('ajaxSend', function (elm, xhr, s) {
        if (s.type == 'POST' && typeof securityToken != 'undefined') {
            if (s.data.length > 0) {
                s.data += "&__RequestVerificationToken=" + encodeURIComponent(securityToken);
            }
            else {
                s.data = "__RequestVerificationToken=" + encodeURIComponent(securityToken);
            }
        }
    });
        
    /***********/
    function preventPropagation(e) {
        e.stopPropagation();
    }
        
    $(document).on("click", ".item", preventPropagation);
        
    /***********/
    function getCategories(e) {
        e.preventDefault();
        e.stopPropagation();
        var $parentCategory = $(this).closest(".parentCategory");
        var parentCategoryName = $parentCategory.attr("id");
        $('.divResult', $parentCategory).load("@Url.Action("CategoryList", "Product")", {
            parentCategory: parentCategoryName,
        });
        $(this).attr("class", "parentCategoryLoaded");
    }

    $(document).on("click", ".parentCategory", getCategories);
        
            /***********/
    function hideCategories(e) {
        e.preventDefault();
        e.stopPropagation();
        $(this).children('.divResult').children().hide();
        $(this).attr("class", "parentCategoryHidChildren");
    }

    $(document).on("click", ".parentCategoryLoaded", hideCategories);
        
    /***********/
    function unHideCategories(e) {
        e.preventDefault();
        e.stopPropagation();
        $(this).find(('.divResult:first')).children().show();
        $(this).attr("class", "parentCategoryLoaded");
    }

    $(document).on("click", ".parentCategoryHidChildren", unHideCategories);

    /***********/
    function getProducts(f) {
        f.preventDefault();
        f.stopPropagation();
        var $category = $(this).closest(".category");
        var categoryName = $category.attr("id");
        $('.divResult', $category).load("@Url.Action("ProductList", "Product")", {
            category: categoryName
        });
        $(this).attr("class", "categoryLoaded");
    }

    $(document).on("click", ".category", getProducts);
        
            /***********/
    function hideProducts(e) {
        e.preventDefault();
        e.stopPropagation();
        $(this).children('.divResult').children().hide();
        $(this).attr("class", "categoryHidChildren");
    }

    $(document).on("click", ".categoryLoaded", hideProducts);
        
    /***********/
    function unHideProducts(e) {
        e.preventDefault();
        e.stopPropagation();
        $(this).find(('.divResult:first')).children().show();
        $(this).attr("class", "categoryLoaded");
    }

    $(document).on("click", ".categoryHidChildren", unHideProducts);

});