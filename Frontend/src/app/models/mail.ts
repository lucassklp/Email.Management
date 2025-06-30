export class Mail {
    id!: number;
    name!: string;
    host!: string;
    port!: number;
    enableSsl!: boolean;
    emailAddress!: string;
    username!: string;
    password!: string
    secret?: string
}