import { UserManager } from "oidc-client";
import config from "./config/index";

class ApplicationUserManager extends UserManager {
  constructor() {
    super({
      //userStore: new WebStorageStateStore({ store: window.localStorage }),
      authority: process.env.NODE_ENV === "development" ? config.auth.dev.authority : config.auth.pro.authority,
      client_id: "myprivate.test.oa.client",
      redirect_uri: process.env.NODE_ENV === "development" ? config.auth.dev.redirect_uri : config.auth.pro.redirect_uri,
      response_type: "code",
      scope: "openid profile account-service",
      post_logout_redirect_uri: process.env.NODE_ENV === "development" ? config.auth.dev.post_logout_redirect_uri : config.auth.pro.post_logout_redirect_uri
    });
  }
  // this.Log.logger = console;
  async login() {
    await this.signinRedirect({ state: { return_uri: window.location.href } });
  }

  async logout() {
    await this.signoutRedirect();
  }
}

const applicationUserManager = new ApplicationUserManager();

export { applicationUserManager as default };