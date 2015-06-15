tinyMCEPopup.requireLangPack();

var netBrowserDialog = {
    init: function (ed) {
        tinyMCEPopup.resizeToInnerSize();
    },

    insert: function (source) {
        var ed = tinyMCEPopup.editor, dom = ed.dom;
        
        tinyMCEPopup.execCommand('mceInsertContent', false, source);

        tinyMCEPopup.close();
    }
};

tinyMCEPopup.onInit.add(netBrowserDialog.init, netBrowserDialog);
