export class Mail {
    id!: number;
    name!: string;
    host!: string;
    port!: number;
    enableSsl!: boolean;
    emailAddress!: string;
    password!: string
    secret?: string
}