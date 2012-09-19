/*
 * Copyright (c) 2008 John Sutherland <john@sneeu.com>
 *
 * Permission to use, copy, modify, and distribute this software for any
 * purpose with or without fee is hereby granted, provided that the above
 * copyright notice and this permission notice appear in all copies.
 *
 * THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
 * WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
 * ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
 * WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
 * ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
 * OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
 */

(function ($) {
    $.fn.shiftClick = function () {
        var lastSelected;
        var checkBoxes = $(this);

        this.each(function () {
            $(this).click(function (ev) {
                if (ev.shiftKey) {
                    var last = checkBoxes.index(lastSelected);
                    var first = checkBoxes.index(this);

                    var start = Math.min(first, last);
                    var end = Math.max(first, last);

                    var chk = lastSelected.checked;
                    for (var i = start; i < end; i++) {
                        checkBoxes[i].checked = chk;
                        //for digital library                        
                        try { ImageCheckSelectionHandler(checkBoxes[i]); } catch (e) { }
                    }
                } else {
                    lastSelected = this;
                }
            })
        });
    };
    /* Custom Shift Click function to be applied to Table Cell (TD) elements */
    $.fn.shiftClickTD = function () {
        var lastSelected;
        var cells = $(this);

        this.each(function () {
            $(this).click(function (ev) {
                if (ev.shiftKey) {
                    var last = cells.index(lastSelected);
                    var first = cells.index(this);
                    var start = Math.min(first, last);
                    var end = Math.max(first, last);
                    //Need to make sure the cell selection order is maintained:                            
                    //Uncheck the last cell selected:
                    try { CellCheckSelectionHandler(cells[end].id); } catch (e) { }

                    //Iterate beginning with Start + 1                    
                    for (var i = start + 1; i < end; i++) {
                        try { CellCheckSelectionHandler(cells[i].id); } catch (e) { }
                    }

                    //Check the last cell selected:
                    try { CellCheckSelectionHandler(cells[end].id); } catch (e) { }
                } else {
                    lastSelected = this;
                }
            })
        });
    };
    $.fn.shiftClickSpot = function () {
        var lastSelected;
        var cells = $(this);
        shiftSelectedCells = [];
        this.each(function () {
            $(this).click(function (ev) {
                if (ev.shiftKey) {
                    var last = cells.index(lastSelected);
                    var first = cells.index(this);

                    var start = Math.min(first, last);
                    var end = Math.max(first, last);

                    for (var i = start; i < end; i++) {
                        $(cells[i]).addClass("selectedSpot");                        
                    }
                } else {
                    lastSelected = this;
                }
            })
        });
    };
})(jQuery);