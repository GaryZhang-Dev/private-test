export default {
    /**
     * @description token在Cookie中存储的天数，默认1天
     */
    cookieExpires: 1,
    /**
     * @description 是否使用国际化，默认为false
     *              如果不使用，则需要在路由中给需要在菜单中展示的路由设置meta: {title: 'xxx'}
     *              用来在菜单中显示文字
     */
    useI18n: false,
    /**
     * @description api请求基础路径
     */
    apiGateway: {
        dev: "http://localhost:8800/",
        pro: "https://api-test.chinalife-gsjy.com/",
    },
    auth: {
        dev: {
            authority: "http://localhost:8900",
            redirect_uri: "http://localhost:8086/signin-callback/",
            post_logout_redirect_uri: "http://localhost:8086/signout-callback/",
        },
        pro: {
            authority: "https://passport-test.chinalife-gsjy.com",
            redirect_uri: "https://platform-oa-test.chinalife-gsjy.com/signin-callback/",
            post_logout_redirect_uri: "https://platform-oa-test.chinalife-gsjy.com/signout-callback/",
        },
    },
    /**
     * @resourcePath 图片等资源的前缀路径
     */
    resourcePath: {
        dev: "https://static-test.chinalife-gsjy.com",
        pro: "https://static-test.chinalife-gsjy.com",
    },
    /**
     * @description 默认打开的首页的路由name值，默认为home
     */
    homeName: "home",
    /**
     * @description 需要加载的插件
     */
    plugin: {
        "error-store": {
            showInHeader: true, // 设为false后不会在顶部显示错误日志徽标
            developmentOff: true, // 设为true后在开发环境不会收集错误信息，方便开发中排查错误
        },
    },
};
