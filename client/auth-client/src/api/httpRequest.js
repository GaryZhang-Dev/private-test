import axios from "axios";
import config from "@/config";

export default async function httpRequest(path, options, otherOptions) {
    const serviceGateWay =
        process.env.NODE_ENV === "development"
            ? config.apiGateway.dev
            : config.apiGateway.pro;
    const url = serviceGateWay + path;
    const defaultOptions = {};
    const newOptions = { ...defaultOptions, ...options };
    if (
        newOptions.methods === "POST" ||
        newOptions.methods === "PUT" ||
        newOptions.methods === "DELETE"
    ) {
        if (!(newOptions.body instanceof FormData)) {
            newOptions.headers = {
                Accept: "application/json",
                "Content-Type": "application/json; charset=utf-8",
                ...newOptions.headers
            };
            newOptions.body = JSON.stringify(newOptions.body);
        } else {
            newOptions.headers = {
                Accept: "application/json",
                ...newOptions.headers
            };
        }
    }
    try {
        var response = await axios({
            method: newOptions.method,
            url: url,
            headers: {
                ...newOptions.headers
            },
            data: {
                ...newOptions.body
            },
            ...otherOptions
        });
        return response.data;
    } catch (e) {
        throw e;
    }
}
