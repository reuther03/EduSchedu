import type { UserRole } from '@/types/enums'

export interface UserIndentityModel {
  token: string;
  userId: string;
  fullName: string;
  email: string;
  role: UserRole;
}
