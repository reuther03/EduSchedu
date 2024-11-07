import type { UserRole } from '@/types/enums'

export interface ILoginResult {
  value: {
    token: string
    userId: string
    fullName: string
    email: string
    role: UserRole
  }
  isSuccess: boolean
  statusCode: number
  message: string
}
