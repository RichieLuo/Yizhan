(function($) {
    $.fn.subStrForPartShow = function (options) {
		var $ele = $(this);
        var $eleTitle = $ele.attr("title");
        console.info($eleTitle);
		var _eleTitle="";
        if ($eleTitle === "") {
			_eleTitle=trim($eleTitle);
		}else{_eleTitle="";}
		
		this.options = $.extend({
			subStart: 0,
			length: 5,			
			originalContent: ""
		}, options);

		function showPartContent() {
			var newStr = "";
			var oldStr = "";
			if(options.length <= 0) {
				options.length = 0
			}
			if(_eleTitle!=="" && options.originalContent === "") {
				oldStr = _eleTitle;
			} else if(options.originalContent !== "") {
				oldStr = trim(options.originalContent);
			} else {
				oldStr = trim($ele.text());
			}			
			newStr = oldStr.substring(options.subStart, options.length);
			if(options.length<oldStr.length){
				newStr += "...<a class='showFull'>详情</a>";
			}		
			$ele.html(newStr);
		}
		function showFullContent() {
			var fullStr = "";
			if(_eleTitle!=="" && options.originalContent === "") {
				fullStr = _eleTitle;
			} else if(_eleTitle!=="" && options.originalContent !== "") {
				fullStr = trim(options.originalContent);
			} else {
				fullStr = _eleTitle;
			}
			fullStr += "<a class='showPart'>收起</a>";
			$ele.html(fullStr);
		}
        function trim(str) {
			return str.replace(/\s|\xA0/g, "");
		}
        $(document).on("click", ".showPart", function (e) {
             e.preventDefault()
			showPartContent();
		})
        $(document).on("click", ".showFull", function (e) {
            e.preventDefault()
			showFullContent();
		})
		showPartContent($ele, options.length);
	};
})(jQuery);