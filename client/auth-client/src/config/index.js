export default {
    useI18n: false,
    /**
     * @description api请求基础路径
     */
    authClientUrl: {
        dev: "https://localhost:8190/",
        pro: "https://localhost:8190/"
    },
    openApiClientUrl: {
        dev: "http://localhost:8091/",
        pro: "https://openapi-test.chinalife-gsjy.com/"
    },
    apiGateway: {
        dev: "https://localhost:8900/",
        pro: "https://localhost:8900/"
    },
    getWechatAuthCode: redirectUri => {
        let appId = "wxcd4828e370c84f5a";
        return `https://open.weixin.qq.com/connect/oauth2/authorize?appid=${appId}&redirect_uri=${encodeURIComponent(redirectUri)}&response_type=code&scope=snsapi_userinfo&state=state#wechat_redirect`;
    },
    getWechatCorpAuthCode: redirectUri => {
        let appId = "wwf6fd7a7c808688e7";
        return `https://open.weixin.qq.com/connect/oauth2/authorize?appid=${appId}&redirect_uri=${encodeURIComponent(redirectUri)}&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect`;
    },

};
