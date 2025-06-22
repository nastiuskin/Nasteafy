export interface IConfig {
  getAuthorization: () => string;
}

export class AuthorizedApiBase {
  protected config: IConfig;

  constructor(config: IConfig) {
    this.config = config;
  }

  protected transformOptions(options: RequestInit): Promise<RequestInit> {
    const token = this.config.getAuthorization();
    return Promise.resolve({
      ...options,
      headers: {
        ...options.headers,
        Authorization: token,
      },
    });
  }
}
