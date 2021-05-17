import httpRequest from "./httpRequest";

export async function Test(params) {
    return await httpRequest(`account-service/test-post`, {
        method: "POST",
        body: {
            ...params
        }
    });
}

export async function GetError(errorId) {
    return await httpRequest(`auth/error?errorId=${errorId}`, {
        method: "GET"
    });
}

export async function Logout(logoutId) {
    return await httpRequest(
        `auth/logout?logoutId=${logoutId}`,
        {
            method: "GET"
        },
        {
            withCredentials: true
        }
    );
}

export async function ModifyPassword(params) {
    return await httpRequest(
        `auth/modify-password`,
        {
            method: "POST",
            body: {
                ...params
            }
        },
        {
            withCredentials: true
        }
    );
}

export async function PasswordLogin(params) {
    return await httpRequest(
        `auth/login/password`,
        {
            method: "POST",
            body: {
                ...params
            }
        },
        {
            withCredentials: true
        }
    );
}
export async function WxCropLogin(params) {
    return await httpRequest(
        `auth/wx-crop`,
        {
            method: "POST",
            body: {
                ...params
            }
        },
        {
            withCredentials: true
        }
    );
}
export async function QrCodeLoginCallback(params) {
    return await httpRequest(
        `auth/qr-code`,
        {
            method: "POST",
            body: {
                ...params
            }
        },
        {
            withCredentials: true
        }
    );
}

export async function PhoneNumberLogin(params) {
    return await httpRequest(
        `auth/login/phone-number`,
        {
            method: "POST",
            body: {
                ...params
            }
        },
        {
            withCredentials: true
        }
    );
}
export async function PhoneNumberLogin2(params) {
    return await httpRequest(
        `auth/login/phone-number2`,
        {
            method: "POST",
            body: {
                ...params
            }
        },
        {
            withCredentials: true
        }
    );
}
export async function WxLogin(params) {
    return await httpRequest(
        `auth/login/wx`,
        {
            method: "POST",
            body: {
                ...params
            }
        },
        {
            withCredentials: true
        }
    );
}

export async function VisitorPasswordLogin(params) {
    return await httpRequest(
        `auth/login/visitor-password`,
        {
            method: "POST",
            body: {
                ...params
            }
        },
        {
            withCredentials: true
        }
    );
}

export async function OpenApiLogin(params) {
    return await httpRequest(
        `auth/login/open-api`,
        {
            method: "POST",
            body: {
                ...params
            }
        },
        {
            withCredentials: true
        }
    );
}

export async function RegisterOpenApi(params) {
    return await httpRequest(`auth/open-api/register`, {
        method: "POST",
        body: {
            ...params
        }
    });
}

export async function GetCaptchaCode(params) {
    return await httpRequest(`auth/get-captcha`, {
        method: "POST",
        body: { ...params },
    });
}
export async function ResetPassword(params) {
    return await httpRequest(`auth/reset-password`, {
        body: params,
        method: "Post",
    });
}
export async function ModifyPhone(params) {
    return await httpRequest(`auth/modify-phone`, {
        body: params,
        method: "Post",
    });
}
export async function ValidPhone(params) {
    return await httpRequest(`auth/valid-phone`, {
        body: params,
        method: "Post",
    });
}

export async function RegisterUser(params) {
    return await httpRequest(`auth/register-user`, {
        body: params,
        method: "Post",
    });
}

