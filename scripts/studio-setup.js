(function ($) {
    $(document.body).ready(function () {
        var loadStudio = function (openMode) {
            var w = window,
                d = document,
                e = d.documentElement,
                g = d.getElementsByTagName('body')[0],
                x = w.innerWidth || e.clientWidth || g.clientWidth,
                settings = window.top.bStudioSettings || {},
                appPath = settings.applicationPath;

            var debugMode = settings.debugMode === true;

            this.siteRoot = window.dnn.getVar("sf_siteRoot", "/");
            this.tabId = parseInt(window.dnn.getVar("sf_tabId", "-1"));

            if (openMode == '1') {
                window.location.href = appPath + 'DesktopModules/BusinessEngine/module-studio/studio.aspx?p={0}&pa={1}&t={2}&m={3}&mt={4}'
                    .replace('{0}', settings.portalID)
                    .replace('{1}', settings.portalAliasID)
                    .replace('{2}', settings.tabID)
                    .replace('{3}', settings.moduleID)
                    .replace('{4}', settings.moduleType)
            }
            else {
                var $container = $('#bEngineStudioApp');

                if ($container.html()) return;

                var src = appPath + 'DesktopModules/BusinessEngine/Module-Studio/container.html';
                src += '?cdv=' + settings.version + (debugMode ? '&t=' + Math.random() : '');

                var $iframe = $('<iframe allowTransparency="true" frameBorder="0" scrolling="false" src="{0}" width="100%" height="100%"></iframe>'.replace('{0}', src));

                $('#bEngineStudioBoxs').hide();

                if (openMode == 2) {
                    $container.append($iframe).appendTo(document.body);
                    $container.addClass('show fullscreen');
                }
                else if (openMode == 3) {
                    $iframe.css('min-height', '500px');
                    $container.append($iframe);
                    $container.addClass('show not-fullscreen');
                }
            }
        }

        $('.box-button').click(function () {
            var openMode = $(this).data('mode');
            loadStudio(parseInt(openMode));
        })
    });
})(jQuery);