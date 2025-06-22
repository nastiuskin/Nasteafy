import { useAuth } from "../hooks/useAuth";

export const apiConfig: IConfig = {
  getAuthorization: () => {
    const { accessToken } = useAuth();
    return accessToken ? `Bearer ${accessToken}` : "";
  },
};

export interface IConfig {
  getAuthorization: () => string;
}

export class AuthorizedApiBase {
  private readonly config: IConfig;

  protected constructor(config: IConfig) {
    this.config = config;
  }

  protected transformOptions = (options: RequestInit): Promise<RequestInit> => {
    options.headers = {
      ...options.headers,
      Authorization: this.config.getAuthorization(),
    };
    return Promise.resolve(options);
  };
}