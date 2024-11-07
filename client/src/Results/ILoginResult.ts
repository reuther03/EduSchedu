interface ILoginResult {
  value: {
    token: string
  }
  isSuccess: boolean
  statusCode: number
  message: string
}
