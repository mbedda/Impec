/**
 * $Id: netFileBrowser
 *
 * @author Ilyas Osmanogullari
 * Http://www.ilyax.com
 * Http://www.ilyax.com/filebrowser/
 * @copyright Copyright © 2010
 */

(function() {
    tinymce.create('tinymce.plugins.netFileBrowser', {
		init : function(ed, url) {
			// Register commands
		    ed.addCommand('mcenetFileBrowser', function () {
				ed.windowManager.open({
					file : url + '/up.aspx',
					width : 660 + parseInt(ed.getLang('netBrowser.delta_width', 0)),
					height : 313 + parseInt(ed.getLang('netBrowser.delta_height', 0)),
					inline : 1
				}, {
					plugin_url : url
				});
			});

			// Register buttons
            ed.addButton('netFileBrowser', { title: 'netFileBrowser', cmd: 'mcenetFileBrowser' });
		},

		getInfo : function() {
			return {
			    longname: 'netFileBrowser',
				author : 'iLyas Osmanogullari',
				authorurl : 'http://ilyax.com',
				infourl : 'http://ilyax.com/filebrowser/',
				version : tinymce.majorVersion + "." + tinymce.minorVersion
			};
		}
	});

	// Register plugin
    tinymce.PluginManager.add('netFileBrowser', tinymce.plugins.netFileBrowser);
})();