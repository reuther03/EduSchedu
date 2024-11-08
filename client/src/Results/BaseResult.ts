export interface BaseResult {
  value: string | null
  isSuccess: boolean
  statusCode: number
  message: string | null
}
